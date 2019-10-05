using ElGuerre.Microservices.Shared.Infrastructure;
using ElGuerre.Microservices.Shipping.Api.Application.Infrastructure;
using ElGuerre.Microservices.Shipping.Api.Application.IntegrationEvents;
using ElGuerre.Microservices.Shipping.Api.Infrastructure;
using GreenPipes;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
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
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Shipping.Api
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
				.AddCustomSwagger()
				.AddCustomMassTransitAzureServiceBus()
				// .AddCustomMassTransitRabbitMQ()
				//.AddCustomDbContext(Configuration);				
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
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "ElGuerre.Microservices.Shipping v1.0.0");
				});

			app.UseHttpsRedirection();
			// app.UseAuthentication();
			app.UseMvc();				
		}
	}

	internal static class CustomExtensionMethods
	{
		private const string DATABASE_CONNECIONSTRING = "DataBaseConnection";

		public static IServiceCollection AddCustomMassTransitAzureServiceBus(this IServiceCollection services)
		{
			services.AddMassTransit(options =>
			{
				// options.AddConsumersFromNamespaceContaining<ElGuerre.Microservices.Sales.Api.Application.Sagas.OrderConsumer>();
				options.AddConsumer<UpdateOrderConsumer>();
				//var sagaRepository = new InMemorySagaRepository<SalesState>();
				//var sagaMachine = new SalesStateMachine();

				options.AddBus(provider => Bus.Factory.CreateUsingAzureServiceBus(cfg =>
				{
					var host = cfg.Host(new Uri("sb://loanmesb2.servicebus.windows.net/"), hostConfig =>
					{
						hostConfig.OperationTimeout = TimeSpan.FromSeconds(5);
						hostConfig.SharedAccessSignature(x =>
						{
							x.KeyName = "RootManageSharedAccessKey";
							x.SharedAccessKey = "5Gl5GvrBEX53QLElJd2nEc+tnVi4RoQzTJ9b4SttDVI=";
						});
					});

					//cfg.ReceiveEndpoint(host, "sales_saga_queue", e =>
					//{
					//	e.StateMachineSaga(sagaMachine, sagaRepository);
					//});
				}));
			});
			
			// services.RegisterInMemorySagaRepository();

			// Required to start Bus Service.
			services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, BusHostedService>();

			return services;
		}

		public static IServiceCollection AddCustomMassTransitRabbitMQ(this IServiceCollection services)
		{
			services.AddMassTransit(options =>
			{					
				// options.AddConsumersFromNamespaceContaining<ElGuerre.Microservices.Sales.Api.Application.Sagas.OrderConsumer>();
				options.AddConsumer<UpdateOrderConsumer>();				

				options.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
				{
					var host = cfg.Host(new Uri("rabbitmq://localhost/"), hostConfig =>
					{
						hostConfig.Username("guest");
						hostConfig.Password("guest");						
					});

					// cfg.ReceiveEndpoint(host, "order_queue", e =>
					cfg.ReceiveEndpoint(e =>
					{
						e.Consumer<UpdateOrderConsumer>();
						// e.UseMessageRetry(x => x.Interval(2, 100));						
					});
				}));
			});

			// Required to start Bus Service.
			services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, BusHostedService>();

			return services;
		}

		public static IServiceCollection AddCustomMVC(this IServiceCollection services)
		{
			services.AddMvc()
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
			services.AddDbContext<ShippingContext>(options =>
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
			// services.AddScoped<IXxxService, XxxService>();
			return services;
		}
	}
}
