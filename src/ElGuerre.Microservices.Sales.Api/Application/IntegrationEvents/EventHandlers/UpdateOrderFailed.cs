using ElGuerre.Microservices.Messages;
using ElGuerre.Microservices.Sales.Api.Application.Models;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Application.IntegrationEvents.EventHanders
{
	public class UpdateOrderFailed :IConsumer<Fault<OrderNameUpdated>>
	{
		private readonly ILogger _logger;

		public UpdateOrderFailed(ILogger<UpdateOrderFailed> logger)
		{
			_logger = logger;
		}

		public async Task Consume(ConsumeContext<Fault<OrderNameUpdated>> context)
		{
			// update the Order
			_logger.LogInformation("Order Update Failed !.");

			throw new Exception("Order Update Failed !");
		}
	}
}
