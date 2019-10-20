using ElGuerre.Microservices.Billing.Api.Infrastructure;
using ElGuerre.Microservices.Billing.Api.Application.Services;
using ElGuerre.Microservices.Billing.Api.Infrastructure;
using ElGuerre.Microservices.Billing.Api.Services;
using ElGuerre.Microservices.Messages;
using ElGuerre.Microservices.Sales.Api.Application.IntegrationHandlers;
using ElGuerre.Microservices.Sales.Api.Application.IntegrationHandlers.Sagas;
using ElGuerre.Microservices.Shared.Infrastructure;
using GreenPipes;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using MassTransit.Azure.ServiceBus.Core.Saga;
using MassTransit.Saga;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using ElGuerre.Microservices.Shared.Behaviors;
using FluentValidation.AspNetCore;

namespace ElGuerre.Microservices.Billing.Api
{
	public class Startup
	{
		public readonly IConfiguration _configuration;
		public readonly BillingSettings _settings;

		public Startup(IConfiguration configuration)
		{
			_configuration = configuration;
			_settings = _configuration.GetSection(Program.AppName).Get<BillingSettings>();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services				
				.AddCustomServices()
				.AddCustomMediatR()
				.AddCustomDbContext(_configuration,_settings)
				.AddCustomSwagger()
				.AddCustomConfiguration(_configuration)
				// .AddCustomMassTransitRabbitMQ()
				.AddCustomMassTransitAzureServiceBus(_settings, isSagaTest: false)
				.AddCustomMVC();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseCors("CorsPolicy");

			app.UseSwagger()
				.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "ElGuerre.Microservices.Billing v1.0.0");
				});

			app.UseHttpsRedirection();
			// app.UseAuthentication();
			app.UseMvc();
		}
	}

	internal static class CustomExtensionMethods
	{
		public static IServiceCollection AddCustomMassTransitAzureServiceBus(
			this IServiceCollection services,
			BillingSettings settings,
			bool isSagaTest)
		{
			services.AddMassTransit(options =>
			{
				options.AddConsumersFromNamespaceContaining<OrderReadyToBillIntegrationEventHandler>();

				options.AddBus(provider => Bus.Factory.CreateUsingAzureServiceBus(cfg =>
				{
					// cfg.EnablePartitioning = true; // Message Broker is enabled !
					cfg.RequiresSession = !isSagaTest;
					cfg.UseJsonSerializer();

					// var host = cfg.Host(settings.EventBusConnectionString, hostConfig =>
					var host = cfg.Host(settings.EventBusUrl, hostConfig =>					
					{
						// hostConfig.ExchangeType = ExchangeType.Topic;
						// hostConfig.TransportType = Microsoft.Azure.ServiceBus.TransportType.AmqpWebSockets;
						// hostConfig.RetryLimit = 3;
						hostConfig.OperationTimeout = TimeSpan.FromMinutes(1);
						
						hostConfig.SharedAccessSignature(x =>
						{
							x.KeyName = settings.EventBusKeyName;
							x.SharedAccessKey = settings.EventBusSharedAccessKey;
							// https://github.com/Azure/azure-service-bus-dotnet/issues/399
							// without this line MassTransit sets the Token TTL to 0.00 instead of null
							x.TokenTimeToLive = TimeSpan.FromDays(1);
						});
						
					});

					//cfg.Send<OrderBilledSuccessfully>(x =>
					//{
					//	x.UseSessionIdFormatter(context =>
					//	{
					//		var sessionId = context.CorrelationId.ToString();
					//		context.SetReplyToSessionId(sessionId);
					//		return sessionId;
					//	});
					//});

					//cfg.Send<IEvent>(x =>
					//// cfg.Send<OrderReadyToBillMessage>(x =>
					//{
					//	x.UseSessionIdFormatter(context =>
					//	{
					//		var sessionId = context.CorrelationId.ToString();
					//		context.SetReplyToSessionId(sessionId);
					//		return sessionId;
					//	});
					//});

					cfg.ReceiveEndpoint(host, "saga_billing_queue", e =>
					{
						//e.UseCircuitBreaker(cb =>
						//{
						//	cb.TrackingPeriod = TimeSpan.FromMinutes(1);
						//	cb.TripThreshold = 15;
						//	cb.ActiveThreshold = 10;
						//	cb.ResetInterval = TimeSpan.FromMinutes(5);
						//});

						e.UseMessageRetry(r =>
						{
							r.Interval(4, TimeSpan.FromSeconds(30));
						});

						// All messages that should be published, are collected in a buffer, which is called “outbox”
						//e.UseInMemoryOutbox();

						//e.SubscribeMessageTopics = true;
						//e.RemoveSubscriptions = true;
						// e.EnablePartitioning = true;  // Message Broker is enabled !
						e.MessageWaitTimeout = TimeSpan.FromMinutes(5);

						e.RequiresSession = !isSagaTest;
						e.StateMachineSaga(provider.GetRequiredService<BillingSagaStateMachine>(),
							provider.GetRequiredService<ISagaRepository<BillingSagaState>>());
					});
				}));
			});

			// services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());

			// Required to start Bus Service.
			services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, BusHostedService>();

			if (isSagaTest)
				services.AddSingleton<ISagaRepository<BillingSagaState>, InMemorySagaRepository<BillingSagaState>>();
			else
				services.AddSingleton<ISagaRepository<BillingSagaState>, MessageSessionSagaRepository<BillingSagaState>>();

			services.AddSingleton<BillingSagaStateMachine>();

			return services;
		}

		public static IServiceCollection AddCustomMassTransitRabbitMQ(this IServiceCollection services)
		{
			services.AddMassTransit(options =>
			{
				// options.AddConsumersFromNamespaceContaining<ElGuerre.Microservices.Sales.Api.Application.Sagas.OrderConsumer>();
				options.AddConsumer<OrderReadyToBillIntegrationEventHandler>();

				options.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
				{
					var host = cfg.Host(new Uri("rabbitmq://localhost/"), hostConfig =>
					{
						hostConfig.Username("guest");
						hostConfig.Password("guest");
					});

					cfg.ReceiveEndpoint(e =>
					{
						//e.Consumer<OrderReadyToBillIntegrationEventHandler>();

					});
				}));
			});

			// Required to start Bus Service.
			services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, BusHostedService>();

			return services;
		}

		public static IServiceCollection AddCustomMediatR(this IServiceCollection services)
		{
			services.AddMediatR(typeof(Startup));
			// services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorPipelineBehavior<,>));

			return services;
		}

		public static IServiceCollection AddCustomConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddOptions();
			services.Configure<BillingSettings>(_ => configuration.GetSection(Program.AppName).Get<BillingSettings>());
			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = context =>
				{
					var problemDetails = new ValidationProblemDetails(context.ModelState)
					{
						Instance = context.HttpContext.Request.Path,
						Status = StatusCodes.Status400BadRequest,
						Detail = "Please refer to the errors property for additional details."
					};

					return new BadRequestObjectResult(problemDetails)
					{
						ContentTypes = { "application/problem+json", "application/problem+xml" }
					};
				};
			});

			return services;
		}

		public static IServiceCollection AddCustomMVC(this IServiceCollection services)
		{
			services.AddMvc()
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
				.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>())
				.AddControllersAsServices();

			services.AddCors(options =>
			{
				options.AddPolicy("CorsPolicy",
					builder => builder
					.SetIsOriginAllowed((host) => true)
					.WithMethods(
						"GET",
						"POST",
						"PUT",
						"DELETE",
						"OPTIONS")
					.AllowAnyHeader()
					.AllowCredentials());
			});

			return services;
		}

		public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration, BillingSettings settings)
		{
			services.AddDbContext<BillingContext>(options =>
			{
				if (configuration.GetValue<bool>("DBInMemory"))
				{
					options.UseInMemoryDatabase("BillingDB",
						(ops) =>
						{

						});
				}
				else
				{
					options.UseSqlServer(settings.OrdersDBConnectionString,
										 sqlServerOptionsAction: sqlOptions =>
										 {
											 sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
											 //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
											 sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
										 });

					// Changing default behavior when client evaluation occurs to throw. 
					// Default in EF Core would be to log a warning when client evaluation is performed.
					options.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
					//Check Client vs. Server evaluation: https://docs.microsoft.com/en-us/ef/core/querying/client-eval
				}
			});

			return services;
		}

		public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
		{
			services.AddSwaggerGen(options =>
			{
				options.DescribeAllEnumsAsStrings();
				options.EnableAnnotations();
				options.SwaggerDoc("v1", new Info
				{
					Version = "v1.0.0",
					Title = "ElGuerre.Microservices.Sales API",
					Description = "API to expose logic for ElGuerre.Microservices.Sales",
					TermsOfService = ""
				});
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				options.IncludeXmlComments(xmlPath);
			});

			return services;
		}

		public static IServiceCollection AddCustomServices(this IServiceCollection services)
		{
			services.AddScoped<IItemsService, ItemsService>();


			// Integration Services
			services.AddScoped<IIntegrationService, IntegrationService>();

			return services;
		}
	}
}
