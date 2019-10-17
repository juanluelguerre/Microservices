using ElGuerre.Microservices.Messages;
using ElGuerre.Microservices.Ordering.Api.Domain.Events;
using ElGuerre.Microservices.Ordering.Api.Domain.Customers;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Customers;
using ElGuerre.Microservices.Shared.Infrastructure;

namespace ElGuerre.Microservices.Ordering.Api.Application.DomainEventHandlers
{
	public class OrderStartedEventHandler : INotificationHandler<OrderStartedDomainEvent>
	{
		private readonly ILogger _logger;
		private readonly ICustomerRepository _customerRepository;		
		private IIntegrationService _integrationService;

		public OrderStartedEventHandler(ILogger<OrderStartedEventHandler> logger, ICustomerRepository customerRepository, IIntegrationService integrationService)
		{
			_logger = logger;
			_customerRepository = customerRepository;
			_integrationService = integrationService;
		}

		public async Task Handle(OrderStartedDomainEvent @event, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Event Handler({nameof(OrderStartedDomainEvent)}) notified for OrderId: {@event.Order.Id}.");

			var cardTypeId = (@event.CardTypeId != 0) ? @event.CardTypeId : 1;

			var customer = await _customerRepository.FindAsync(@event.UserId);

			bool customerOriginallyExisted = (customer == null) ? false : true;

			if (!customerOriginallyExisted)
			{
				customer = new Customer(@event.UserId, @event.UserName);
			}

			customer.VerifyOrAddPaymentMethod(cardTypeId,
										   $"Payment Method on {DateTime.UtcNow}",
										   @event.CardNumber,										   
										   @event.CardHolderName,
										   @event.CardExpiration,
										   @event.Order.Id);

			var customerUpdated = customerOriginallyExisted ?
				_customerRepository.Update(customer) :
				_customerRepository.Add(customer);

			await _customerRepository.UnitOfWork
				.SaveEntitiesAsync(cancellationToken);

			// var orderStatusChangedTosubmittedIntegrationEvent = new OrderStatusChangedToSubmittedIntegrationEvent(@event.Order.Id, @event.Order.OrderStatus.Name, customer.Name);
			// await _orderingIntegrationEventService.AddAndSaveEventAsync(orderStatusChangedTosubmittedIntegrationEvent);			

			_logger.LogTrace($"Customer {customerUpdated.Id} and related payment method were validated or updated for orderId: {@event.Order.Id}.");
		}
	}
}
