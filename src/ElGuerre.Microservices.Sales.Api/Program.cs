using ElGuerre.Microservices.Sales.Api.Application.Infrastructure;
using ElGuerre.Microservices.Shared.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api
{
	public static class Program
	{
		private static readonly string Namespace = typeof(Program).Namespace;
		private static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);

		public static int Main(string[] args)
		{
			var configuration = GetConfiguration();

			Log.Logger = CreateSerilogLogger(configuration);

			try
			{
				Log.Information("Configuring web host ({ApplicationContext})...", AppName);
				var host = BuildWebHost(configuration, args);

				Log.Information("Applying migrations ({ApplicationContext})...", AppName);
				host.MigrateDbContext<OrdersContext>((context, services) =>
				{
					var env = services.GetService<IHostingEnvironment>();
					var settings = services.GetService<IOptions<OrdersSettings>>();
					var logger = services.GetService<ILogger<OrdersContextSeed>>();

					new OrdersContextSeed()
						.SeedAsync(context, env, settings, logger)
						.Wait();
				});
				// .MigrateDbContext<IntegrationEventLogContext>((_, __) => { });

				Log.Information("Starting web host ({ApplicationContext})...", AppName);
				host.Run();

				return 0;
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);
				return 1;
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		private static IWebHost BuildWebHost(IConfiguration configuration, string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.CaptureStartupErrors(false)
				.UseStartup<Startup>()
				.UseContentRoot(Directory.GetCurrentDirectory())
				.ConfigureAppConfiguration((host, config) =>
				{
					if (host.HostingEnvironment.IsDevelopment())
						config.AddUserSecrets<Startup>();
				})
				.UseWebRoot("Assets")
				.UseConfiguration(configuration)
				.UseSerilog()
				.Build();

		private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
		{
			var seqServerUrl = configuration["Serilog:SeqServerUrl"];
			var logstashUrl = configuration["Serilog:LogstashgUrl"];
			return new LoggerConfiguration()
				.MinimumLevel.Verbose()
				.Enrich.WithProperty("ApplicationContext", AppName)
				.Enrich.FromLogContext()
				.WriteTo.Console()
				// .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
				// .WriteTo.Http(string.IsNullOrWhiteSpace(logstashUrl) ? "http://logstash:8080" : logstashUrl)
				.ReadFrom.Configuration(configuration)
				.CreateLogger();
		}

		private static IConfiguration GetConfiguration()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddEnvironmentVariables();

			var config = builder.Build();

			//if (config.GetValue<bool>("UseVault", false))
			//{
			//	builder.AddAzureKeyVault(
			//		$"https://{config["Vault:Name"]}.vault.azure.net/",
			//		config["Vault:ClientId"],
			//		config["Vault:ClientSecret"]);
			//}

			return builder.Build();
		}
	}
}
