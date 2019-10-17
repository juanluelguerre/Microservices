using ElGuerre.Microservices.Billing.Api;
using ElGuerre.Microservices.Billing.Api.Domain.Events;
using ElGuerre.Microservices.Messages;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Application.IntegrationHandlers
{
	public class OrderReadyToBillIntegrationEventHandler : IConsumer<OrderReadyToBillMessage>
	{
		private readonly ILogger _logger;
		private readonly IMediator _mediator;

		public OrderReadyToBillIntegrationEventHandler(ILogger<OrderReadyToBillIntegrationEventHandler> logger, IMediator mediator)
		{
			_logger = logger;
			_mediator = mediator;
		}

		public async Task Consume(ConsumeContext<OrderReadyToBillMessage> context)
		{

			_logger.LogInformation("Starting process to pay...");

			// TODO: Wait to simulate payment transaction;
			await Task.Delay(5000);

			// enviar otro servicio
			/// await _integrationService.PublishToEventBusAsync(new OrderBillSuccededMessage(request.OrderId), Program.AppName);


			// opction: pagar
			// Itemservic.adfsdfdsafaeafdas();

		}
	}
}
