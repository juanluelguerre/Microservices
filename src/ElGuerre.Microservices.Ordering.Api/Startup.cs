using ElGuerre.Microservices.Messages;
using ElGuerre.Microservices.Messages.Orders;
using ElGuerre.Microservices.Ordering.Api.Application;
using ElGuerre.Microservices.Ordering.Api.Application.Commands;
using ElGuerre.Microservices.Ordering.Api.Application.IntegrationHandlers;
using ElGuerre.Microservices.Ordering.Api.Application.IntegrationHandlers.Sagas;
using ElGuerre.Microservices.Ordering.Api.Application.Models;
using ElGuerre.Microservices.Ordering.Api.Application.Validations;
using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders;
using ElGuerre.Microservices.Ordering.Api.Domain.Customers;
using ElGuerre.Microservices.Ordering.Api.Infrastructure;
using ElGuerre.Microservices.Ordering.Api.Infrastructure;
using ElGuerre.Microservices.Ordering.Api.Infrastructure.Filters;
using ElGuerre.Microservices.Ordering.Api.Infrastructure.Repositories;
using ElGuerre.Microservices.Shared.Behaviors;
using ElGuerre.Microservices.Shared.Infrastructure;
using FluentValidation;
using FluentValidation.AspNetCore;
using GreenPipes;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using MassTransit.Azure.ServiceBus.Core.Saga;
using MassTransit.Context;
using MassTransit.Saga;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
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

namespace ElGuerre.Microservices.Ordering.Api
{
	public class Startup
	{
		public readonly IConfiguration _configuration;
		public readonly OrderingSettings _settings;

		public Startup(IConfiguration configuration)
		{			
			_configuration = configuration;
			_settings = _configuration.GetSection(Program.AppName).Get<OrderingSettings>();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddCustomDbContext(_configuration, _settings)
				.AddCustomServices()				
				.AddSwagger()
				.AddCustomConfiguration(_configuration)
				.AddCustomMediatR()
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
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "ElGuerre.Microservices.Sales v1.0.0");
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
			OrderingSettings settings, 
			bool isSagaTest)
		{
			services.AddMassTransit(options =>
			{
				// Integration Events as Masstransit Consumers.
				// options.AddConsumersFromNamespaceContaining<OrderToPayConsumer>();				

				options.AddBus(provider => Bus.Factory.CreateUsingAzureServiceBus(cfg =>
				{
					// cfg.EnablePartitioning = true; // Message Broker is enabled !
					cfg.RequiresSession = !isSagaTest;
					cfg.UseJsonSerializer();

					// var host = cfg.Host(settings.EventBusConnectionString, hostConfig =>
					var host = cfg.Host(new Uri(settings.EventBusUrl), hostConfig =>
					{
						// hostConfig.ExchangeType = ExchangeType.Topic;
						// hostConfig.TransportType = Microsoft.Azure.ServiceBus.TransportType.AmqpWebSockets;
						//hostConfig.RetryLimit = 3;
						//hostConfig.OperationTimeout = TimeSpan.FromMinutes(1);
						hostConfig.SharedAccessSignature(x =>
						{
							x.KeyName = settings.EventBusKeyName;
							x.SharedAccessKey = settings.EventBusSharedAccessKey;
							// https://github.com/Azure/azure-service-bus-dotnet/issues/399
							// without this line MassTransit sets the Token TTL to 0.00 instead of null
							x.TokenTimeToLive = TimeSpan.FromDays(1);
						});
					});

					// cfg.SubscriptionEndpoint(host, "buscriptionxxx", "topicxxx", config => { config.xxx });					

					// cfg.Send<IEvent>(x =>
					cfg.Send<OrderReadyToBillMessage>(x =>
					{
						x.UseSessionIdFormatter(context =>
						{
							var sessionId = context.CorrelationId.ToString();
							context.SetReplyToSessionId(sessionId);
							return sessionId;
						});
					});

					cfg.ReceiveEndpoint(host, "saga_sales_queue", e =>
					{
						e.UseCircuitBreaker(cb =>
						{
							cb.TrackingPeriod = TimeSpan.FromMinutes(1);
							cb.TripThreshold = 15;
							cb.ActiveThreshold = 10;
							cb.ResetInterval = TimeSpan.FromMinutes(5);
						});

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

						// e.Consumer<OrderToPayConsumer>( );
						// e.Consumer(() => provider.GetRequiredService<IConsumer<OrderToPayConsumer>>());

						e.StateMachineSaga(provider.GetRequiredService<OrdersSagaStateMachine>(),
							provider.GetRequiredService<ISagaRepository<OrderSagaState>>());						
					});
				}));
			});

			// services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());

			// Required to start Bus Service.
			services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, BusHostedService>();

			if (isSagaTest)
				services.AddSingleton<ISagaRepository<OrderSagaState>, InMemorySagaRepository<OrderSagaState>>();
			else
				services.AddSingleton<ISagaRepository<OrderSagaState>, MessageSessionSagaRepository<OrderSagaState>>();

			services.AddSingleton<OrdersSagaStateMachine>();

			return services;
		}

		public static IServiceCollection AddCustomMassTransitRabbitMQ(this IServiceCollection services, ILoggerFactory loggerFactory, bool isSagaTest)
		{
			// https://masstransit-project.com/MassTransit/advanced/sagas/automatonymous.html

			services.AddMassTransit(options =>
			{
				options.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
				{
					var host = cfg.Host(new Uri("rabbitmq://localhost/"), hostConfig =>
					{
						hostConfig.Username("guest");
						hostConfig.Password("guest");
					});

					// cfg.UseRetry(Retry.Immediate(5));

					cfg.ReceiveEndpoint(host, "sales_saga_queue", e =>
					{
						// e.StateMachineSaga(sagaMachine, sagaRepository);
						e.StateMachineSaga(provider.GetRequiredService<OrdersSagaStateMachine>(),
							provider.GetRequiredService<ISagaRepository<OrderSagaState>>());
					});



					cfg.UseCircuitBreaker(cb =>
					{
						cb.TrackingPeriod = TimeSpan.FromMinutes(1);
						cb.TripThreshold = 15;
						cb.ActiveThreshold = 10;
						cb.ResetInterval = TimeSpan.FromMinutes(5);
					});

					// for testing, to make it easy
					// cfg.UseInMemoryMessageScheduler(); 

					// or, configure the endpoints by convention
					// cfg.ConfigureEndpoints(provider);
				}));

				// Conventions
				// EndpointConvention.Map<Order>(new Uri($"rabbitmq://localhost/{queueName}"));				
			});

			// Required to start Bus Service.
			services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, BusHostedService>();

			if (isSagaTest)
				services.AddSingleton<ISagaRepository<OrderSagaState>, InMemorySagaRepository<OrderSagaState>>();
			else
				services.AddSingleton<ISagaRepository<OrderSagaState>, MessageSessionSagaRepository<OrderSagaState>>();

			services.AddSingleton<OrdersSagaStateMachine>();

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
			services.Configure<OrderingSettings>(_ => configuration.GetSection(Program.AppName).Get<OrderingSettings>());
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
			services.AddMvc(options =>
			{
				// Custom Filter to validate BadRequests for all Controllers.
				options.Filters.Add(typeof(ValidateModelState));
			})
			.SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
			.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>())
			.AddControllersAsServices();

			 services.AddTransient(typeof(IValidator<OrderModelValidator>), typeof(OrderModelValidator));

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

		public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration, OrderingSettings settings)
		{
			services.AddDbContext<OrderingContext>(options =>
			{
				if (configuration.GetValue<bool>("DBInMemory"))
				{
					options.UseInMemoryDatabase("OrdersDB",
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

		public static IServiceCollection AddSwagger(this IServiceCollection services)
		{
			services.AddSwaggerGen(options =>
			{
				options.DescribeAllEnumsAsStrings();
				options.EnableAnnotations();
				options.SwaggerDoc("v1", new Info
				{
					Version = "v1.0.0",
					Title = "ElGuerre.Microservices.Sales",
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
			services.AddTransient<IOrderRepository, OrdersRepository>();
			services.AddTransient<ICustomerRepository, CustomerRepository>();

			// Integration Services
			services.AddTransient<IIntegrationService, IntegrationService>();

			return services;
		}
	}
}

