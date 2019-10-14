using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Application.Infrastructure
{
	public class OrdersContextSeed
	{

		public async Task SeedAsync(OrdersContext dbContext,
			IHostingEnvironment env,
			IOptions<OrdersSettings> settings,
			ILogger<OrdersContextSeed> logger)
		{
			var policy = CreatePolicy(logger, nameof(OrdersContextSeed));

			await policy.ExecuteAsync(async () =>
			{
				// TODO: Sample code. Replace and use the the correct one.

				dbContext.Orders.AddRange(
					new Domain.Order() { Id = 1, Name = "Order 1" },
					new Domain.Order() { Id = 2, Name = "Order 2" },
					new Domain.Order() { Id = 3, Name = "Order 3" },
					new Domain.Order() { Id = 4, Name = "Order 4" },
					new Domain.Order() { Id = 5, Name = "Order 5" }
				);

				await dbContext.SaveChangesAsync();
			});
		}

		private AsyncRetryPolicy CreatePolicy(ILogger<OrdersContextSeed> logger, string prefix, int retries = 3)
		{
			var policy = Policy.Handle<SqlException>().
				WaitAndRetryAsync(
					retryCount: retries,
					sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
					onRetry: (exception, timeSpan, retry, ctx) =>
					{
						logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
					}
				);

			return policy;
		}
	}
}
