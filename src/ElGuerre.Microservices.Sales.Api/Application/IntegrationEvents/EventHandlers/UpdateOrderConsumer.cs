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
	public class UpdateOrderConsumer : IConsumer<OrderPlaced>
	{
		private readonly ILogger _logger;

		public UpdateOrderConsumer(ILogger<UpdateOrderConsumer> logger)
		{
			_logger = logger;
		}

		public async Task Consume(ConsumeContext<OrderPlaced> context)
		{
			var sessionId = context.SessionId();

			_logger.LogInformation($"Order Shales with {context.Message.OrderId} accepted.");

			await Task.FromResult(0);
		}
	}
}
