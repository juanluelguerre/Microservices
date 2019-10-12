using ElGuerre.Microservices.Messages;
using ElGuerre.Microservices.Sales.Api.Application.Infrastructure;
using ElGuerre.Microservices.Sales.Api.Application.IntegrationEvents;
using ElGuerre.Microservices.Sales.Api.Application.IntegrationEvents.EventHanders;
using ElGuerre.Microservices.Sales.Api.Application.Models;
using ElGuerre.Microservices.Sales.Api.Application.Sagas;
using ElGuerre.Microservices.Sales.Api.Application.Services;
using ElGuerre.Microservices.Sales.Api.Infrastructure;
using ElGuerre.Microservices.Sales.Api.Infrastructure.Filters;
using ElGuerre.Microservices.Sales.Api.Services;
using ElGuerre.Microservices.Shared.Infrastructure;
using GreenPipes;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using MassTransit.Azure.ServiceBus.Core.Saga;
using MassTransit.Context;
using MassTransit.Saga;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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


namespace ElGuerre.Microservices.Sales.Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddCustomDbContext(Configuration)
				.AddCustomServices()
				.AddCustomMediatR()
				.AddSwagger()
				// .AddCustomDbContext(Configuration)
				// .AddCustomMassTransitRabbitMQ()
				.AddCustomMassTransitAzureServiceBus()
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
		private const string DATABASE_CONNECIONSTRING = "DataBaseConnection";

		public static IServiceCollection AddCustomMediatR(this IServiceCollection services)
		{
			services.AddMediatR(typeof(Startup));
			return services;
		}

		public static IServiceCollection AddCustomMassTransitAzureServiceBus(this IServiceCollection services)
		{
			bool isSagaTest = false;
			// MessageCorrelation.UseCorrelationId<OrderPlaced>(x => x.CorrelationId);

			services.AddMassTransit(options =>
			{
				options.AddConsumersFromNamespaceContaining<UpdateOrderConsumer>();

				options.AddBus(provider => Bus.Factory.CreateUsingAzureServiceBus(cfg =>
				{
					// cfg.EnablePartitioning = true; // Message Broker is enabled !
					cfg.RequiresSession = !isSagaTest;
					// cfg.UseJsonSerializer();

					// var host = cfg.Host("#CONNECTION_STRING#", hostConfig =>
					var host = cfg.Host(new Uri("sb://loanmesb2.servicebus.windows.net/"), hostConfig =>
					{
						// hostConfig.ExchangeType = ExchangeType.Topic;
						hostConfig.TransportType = Microsoft.Azure.ServiceBus.TransportType.AmqpWebSockets;
						hostConfig.RetryLimit = 3;
						hostConfig.OperationTimeout = TimeSpan.FromMinutes(1);
						hostConfig.SharedAccessSignature(x =>
						{
							x.KeyName = "RootManageSharedAccessKey";
							x.SharedAccessKey = "5Gl5GvrBEX53QLElJd2nEc+tnVi4RoQzTJ9b4SttDVI=";

							// https://github.com/Azure/azure-service-bus-dotnet/issues/399
							// without this line MassTransit sets the Token TTL to 0.00 instead of null
							x.TokenTimeToLive = TimeSpan.FromDays(1);							
						});
					});

					// cfg.SubscriptionEndpoint(host, "buscriptionxxx", "topicxxx", config => { config.xxx });					

					cfg.Send<OrderPlaced>(x =>
					{						
						x.UseSessionIdFormatter(context =>
						{
							var sessionId = context.CorrelationId.ToString();
							context.SetReplyToSessionId(sessionId);
							return sessionId;
						});
					});

					//cfg.Send<OrderBilled>(x =>
					//{					
					//	x.UseSessionIdFormatter(context =>
					//	{
					//		var sessionId = context.CorrelationId.ToString();
					//		context.SetReplyToSessionId(sessionId);
					//		return sessionId;
					//	});
					//});

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

						ISagaRepository<SalesState> sagaRepository;
						if (isSagaTest)
						{
							sagaRepository = new InMemorySagaRepository<SalesState>();
						}
						else
						{
							e.RequiresSession = true;
							sagaRepository = new MessageSessionSagaRepository<SalesState>();
						}

						// All messages that should be published, are collected in a buffer, which is called “outbox”
						//e.UseInMemoryOutbox();

						//e.SubscribeMessageTopics = true;
						//e.RemoveSubscriptions = true;
						// e.EnablePartitioning = true;  // Message Broker is enabled !
						e.MessageWaitTimeout = TimeSpan.FromMinutes(5);

						var sagaMachine = new SalesStateMachine();
						e.StateMachineSaga(sagaMachine, sagaRepository);
						//e.Saga<UpdatePolicySaga>(new MessageSessionSagaRepository<UpdatePolicySaga>)
					});
				}));
			});


			services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());


			// Required to start Bus Service.
			services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, BusHostedService>();
			// services.AddSingleton<ISagaRepository<SalesState>, MessageSessionSagaRepository<SalesState>>();

			return services;
		}

		public static IServiceCollection AddCustomMassTransitRabbitMQ(this IServiceCollection services)
		{
			// https://masstransit-project.com/MassTransit/advanced/sagas/automatonymous.html

			services.AddMassTransit(options =>
			{
				// options.AddConsumersFromNamespaceContaining<ElGuerre.Microservices.Sales.Api.Application.Sagas.OrderConsumer>();
				// options.AddConsumer<UpdateOrderConsumer>();
				var sagaRepository = new InMemorySagaRepository<SalesState>();
				var sagaMachine = new SalesStateMachine();

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
						//e.BindMessageExchanges = false;						
						//e.Bind("submitorder", x =>
						//{
						//	x.ExchangeType = ExchangeType.Direct;
						//});
						// e.Bind<OrderConsumer>();						

						// e.Consumer<UpdateOrderConsumer>();						
						// e.UseMessageRetry(x => x.Interval(2, 100));

						e.StateMachineSaga(sagaMachine, sagaRepository);
					});

					//cfg.ReceiveEndpoint(host, "shiping_queue", e =>
					//{
					//	e.PrefetchCount = 1;
					//	// e.Consumer(() => 
					//});

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

			// services.AddScoped<PublishOrderEventActivity>();
			services.RegisterInMemorySagaRepository();

			// Required to start Bus Service.
			services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, BusHostedService>();

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

		public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<OrderContext>(options =>
			{
				var dbInMemory = configuration.GetValue<bool>("DBInMemory");

				if (dbInMemory)
				{
					options.UseInMemoryDatabase("Modulo1DB",
						(ops) =>
						{

						});
				}
				else
				{
					options.UseSqlServer(configuration.GetConnectionString(DATABASE_CONNECIONSTRING),
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
			services.AddScoped<IOrdersService, OrdersService>();
			return services;
		}
	}
}
