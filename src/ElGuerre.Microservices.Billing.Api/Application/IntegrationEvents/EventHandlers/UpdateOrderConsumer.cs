using ElGuerre.Microservices.Messages;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Application.IntegrationEvents.EventHanders
{
	/// <summary>
	/// This Comsumer is just an example for Billing service to know when a Product has been ordered ! Just a sample to Broadcast messages !!
	/// </summary>
	public class UpdateOrderConsumer : IConsumer<OrderPlaced>
	{
		public async Task Consume(ConsumeContext<OrderPlaced> context)
		{		
			Console.WriteLine($"Order Placed on Billing Services with {context.Message.OrderId} accepted.");
			await Task.FromResult(0);
		}
	}
}
