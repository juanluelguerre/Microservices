using ElGuerre.Microservices.Messages;
using ElGuerre.Microservices.Sales.Api.Application.Models;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Application.IntegrationEvents.EventHanders
{

	public class UpdateOrderFailed :IConsumer<Fault<OrderNameUpdated>>
	{
		public async Task Consume(ConsumeContext<Fault<OrderNameUpdated>> context)
		{
			// update the Order
			Console.WriteLine("Order Update Fail");
			throw new Exception("Order Update Fail");
		}
	}
}
