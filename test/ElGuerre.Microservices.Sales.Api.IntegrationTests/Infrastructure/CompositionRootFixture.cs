using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace ElGuerre.Microservices.Sales.Api.IntegrationTests.Infrastructure
{
	public class CompositionRootFixture
	{		
		private readonly TestServer _server;
		public HttpClient Client { get; }

		public CompositionRootFixture()
		{
			_server = new TestServer(new WebHostBuilder()
				.UseEnvironment("Test")
				.ConfigureAppConfiguration((context, configBuilder) =>
				{
					configBuilder
						.SetBasePath(context.HostingEnvironment.ContentRootPath)
						.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
						.AddEnvironmentVariables();
				})
				.UseStartup<Startup>());

			Client = _server.CreateClient();
		}

		~CompositionRootFixture()
		{
			Client.Dispose();
			_server.Dispose();
		}
	}
}
