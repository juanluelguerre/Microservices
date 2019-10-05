using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElGuerre.Microservices.Messages;

namespace ElGuerre.Microservices.Sales.Api.Application.IntegrationEvents.EventHanders
{
	public class UpdateOrderToBilledConsumer : IConsumer<OrderBilled>
	{
		public async Task Consume(ConsumeContext<OrderBilled> context)
		{
			Console.WriteLine($"Order Billed with {context.Message.OrderId} accepted.");
			await Task.FromResult(0);
		}
	}
}
