using ElGuerre.Microservices.Ordering.Api.Application.Extensions;
using ElGuerre.Microservices.Ordering.Api.Application.Queries;
using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Customers;
using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Application.Commands
{
	public class OrderSetToBilledCommandHandler : IRequestHandler<OrderSetToBilledCommand, bool>
	{
		private readonly ILogger _logger;
		private readonly IOrderRepository _orderRepository;
		private readonly IOrderQuery _orderQuery;

		public OrderSetToBilledCommandHandler(
			ILogger<OrderSetToBilledCommandHandler> logger, 
			IOrderQuery orderQuery, 
			IOrderRepository orderRepository)
		{
			_logger = logger;
			_orderQuery = orderQuery;
			_orderRepository = orderRepository;
		}

		public async Task<bool> Handle(OrderSetToBilledCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Handle({nameof(OrderSetToBilledCommandHandler)}) -> {command}");

			var orderToUpdate = await _orderQuery.FindByIdAsync(command.OrderId);
			orderToUpdate.SetPaidStatus();

			return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
		}
	}
}
