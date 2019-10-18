using ElGuerre.Microservices.Ordering.Api.Application.Models;
using Aggregate = ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders;
using ElGuerre.Microservices.Ordering.Api.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Application.Commands
{
	public class OrderCreateCommandHandler : IRequestHandler<OrderCreateCommand, bool>
	{
		private readonly Aggregate.IOrderRepository _orderRepository;
		private readonly ILogger<OrderCreateCommandHandler> _logger;
		
		public OrderCreateCommandHandler(ILogger<OrderCreateCommandHandler> logger, Aggregate.IOrderRepository orderRepository)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));									
		}

		public async Task<bool> Handle(OrderCreateCommand message, CancellationToken cancellationToken)
		{
			// Add/Update the Buyer AggregateRoot
			// DDD patterns comment: Add child entities and value-objects through the Order Aggregate-Root
			// methods and constructor so validations, invariants and business logic 
			// make sure that consistency is preserved across the whole aggregate
			var address = new Address(message.Street, message.City, message.Country, message.ZipCode);
			var order = new Aggregate.Order(message.UserId, message.UserName, address, message.CardTypeId, message.CardNumber, message.CardHolderName, message.CardExpiration);

			foreach (var item in message.OrderItems)
			{
				order.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.Discount, item.Units);
			}

			_logger.LogInformation("----- Creating Order - Order: {@Order}", order);

			_orderRepository.Add(order);

			return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
		}
	}
}
