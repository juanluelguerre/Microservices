using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Customers;
using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders;
using ElGuerre.Microservices.Ordering.Api.Domain.ValueObjects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Infrastructure
{
	public class OrderingContextSeed
	{

		public async Task SeedAsync(OrderingContext dbContext,
			IHostingEnvironment env,
			IOptions<OrdersSettings> settings,
			ILogger<OrderingContextSeed> logger)
		{
			var policy = CreatePolicy(logger, nameof(OrderingContextSeed));

			await policy.ExecuteAsync(async () =>
			{
				if (dbContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
				{
					dbContext.Database.Migrate();
				}				

				// TODO: Sample code. Replace and use the the correct one.

				var address = new Address("Street 1", "City 1", "Spain", "28000");

				dbContext.Payments.AddRange(
					new PaymentMethod(1, "Amex", "100100XAB120", "Juanlu", DateTime.Now.AddDays(60)),
					new PaymentMethod(2, "Visa", "200200XAB120", "Juanlu", DateTime.Now.AddMonths(6))
					);

				dbContext.OrderStatus.AddRange(
					new OrderStatus(1, "Submittted"),
					new OrderStatus(2, "AwaitingValidation"),
					new OrderStatus(3, "StockConfirmed"),
					new OrderStatus(4, "Paid"),
					new OrderStatus(5, "Shipped"),
					new OrderStatus(6, "Cancelled")
				);


				dbContext.Orders.AddRange(
					new Order(Guid.NewGuid().ToString("N"), "Juanlu", address, 1, "AB100100100", "Juanlu", DateTime.Now.AddYears(1))
				);
				//await dbContext.SaveChangesAsync();

				await dbContext.SaveChangesAsync();
			});
		}

		private AsyncRetryPolicy CreatePolicy(ILogger<OrderingContextSeed> logger, string prefix, int retries = 3)
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
