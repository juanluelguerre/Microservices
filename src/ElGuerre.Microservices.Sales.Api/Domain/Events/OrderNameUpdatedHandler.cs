using ElGuerre.Microservices.Messages;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Domain.Events
{
	public class OrderNameUpdatedHandler : INotificationHandler<OrderNameUpdated>
	{
		private readonly ILogger _logger;
		private readonly IBus _bus;

		public OrderNameUpdatedHandler(ILogger<OrderNameUpdatedHandler> logger, IBus bus)
		{
			_logger = logger;
			_bus = bus;
		}

		public async Task Handle(OrderNameUpdated order, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Event Handler({nameof(OrderNameUpdatedHandler)}) notified for OrderId: {order.OrderId} - '{order.Name}'.");

			//
			// Integration Services Operation
			//
			var @event = new OrderPlaced() { OrderId = order.OrderId, Name = order.Name };
			@event.CorrelationId = Guid.NewGuid();
			await _bus.Publish(@event);

			_logger.LogInformation($"Event Handler({nameof(OrderNameUpdatedHandler)}) published with CorrelationId: '{@event.CorrelationId}'.");
		}
	}
}
