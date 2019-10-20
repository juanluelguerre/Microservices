using ElGuerre.Microservices.Ordering.Api.Application.Extensions;
using ElGuerre.Microservices.Ordering.Api.Application.Queries;
using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders;
using ElGuerre.Microservices.Ordering.Api.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Application.DomainEventsHandlers
{
	public class OrderUpdateWhenCustomerAndPaymentMethodVerifiedDomainEventHandler
				   : INotificationHandler<CustomerAndPaymentMethodVerifiedDomainEvent>
	{
		private readonly IMediator _mediator;
		private readonly IOrderRepository _orderRepository;
		private readonly ILogger _logger;

		public OrderUpdateWhenCustomerAndPaymentMethodVerifiedDomainEventHandler(
			ILogger<OrderUpdateWhenCustomerAndPaymentMethodVerifiedDomainEventHandler> logger,
			IOrderRepository orderRepository,
		IMediator mediator)
		{			
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		}

		// Domain Logic comment:
		// When the Buyer and Buyer's payment method have been created or verified that they existed, 
		// then we can update the original Order with the BuyerId and PaymentId (foreign keys)
		public async Task Handle(CustomerAndPaymentMethodVerifiedDomainEvent @event, CancellationToken cancellationToken)
		{
			var command = new OrderByIdQuery(@event.OrderId);
			var model = await _mediator.Send(command);

			var orderToUpdate = model.ToOrder();
			orderToUpdate.SetCustomerId(@event.Customer.Id);
			orderToUpdate.SetPaymentId(@event.Payment.Id);
						
			await _orderRepository.UnitOfWork.SaveEntitiesAsync();

			_logger.LogTrace($"Order with Id: {@event.OrderId} has been successfully updated with a payment method {nameof(@event.Payment)} ({@event.Payment.Id})");
		}
	}
}