using ElGuerre.Microservices.Ordering.Api.Domain.Events;
using ElGuerre.Microservices.Ordering.Api.Domain.Orders;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Application.DomainEventsHandlers
{
	public class UpdateOrderWhenCustomerAndPaymentMethodVerifiedDomainEventHandler
				   : INotificationHandler<CustomerAndPaymentMethodVerifiedDomainEvent>
	{
		private readonly IOrdersRepository _orderRepository;
		private readonly ILogger _logger;

		public UpdateOrderWhenCustomerAndPaymentMethodVerifiedDomainEventHandler(
			ILogger<UpdateOrderWhenCustomerAndPaymentMethodVerifiedDomainEventHandler> logger,
			IOrdersRepository orderRepository)
		{
			_orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		// Domain Logic comment:
		// When the Buyer and Buyer's payment method have been created or verified that they existed, 
		// then we can update the original Order with the BuyerId and PaymentId (foreign keys)
		public async Task Handle(CustomerAndPaymentMethodVerifiedDomainEvent @event, CancellationToken cancellationToken)
		{
			var orderToUpdate = await _orderRepository.GetByIdAsync(@event.OrderId);
			orderToUpdate.SetCustomerId(@event.Customer.Id);
			orderToUpdate.SetPaymentId(@event.Payment.Id);
						
			await _orderRepository.UnitOfWork.SaveEntitiesAsync();

			_logger.LogTrace($"Order with Id: {@event.OrderId} has been successfully updated with a payment method {nameof(@event.Payment)} ({@event.Payment.Id})");
		}
	}
}