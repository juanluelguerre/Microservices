using ElGuerre.Microservices.Messages;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Shipping.Api.Application.IntegrationEvents
{
	public class UpdateOrderConsumer : IConsumer<OrderBilled>
	{
		public async Task Consume(ConsumeContext<OrderBilled> context)
		{
			Console.WriteLine($"Order placed in a Shipping service with {context.Message.OrderId} accepted.");
			await Task.FromResult(0);
		}
	}
}
