using ElGuerre.Microservices.Messages;
using ElGuerre.Microservices.Sales.Api.Application.Models;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Application.IntegrationEvents.EventHanders
{
	public class UpdateOrderConsumer : IConsumer<OrderPlaced>
	{
		public async Task Consume(ConsumeContext<OrderPlaced> context)
		{
			var sessionId = context.SessionId();

			Console.WriteLine($"Order Shales with {context.Message.OrderId} accepted.");
			await Task.FromResult(0);
		}
	}
}
