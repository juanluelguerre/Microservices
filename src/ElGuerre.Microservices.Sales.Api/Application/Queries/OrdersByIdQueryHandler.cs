using ElGuerre.Microservices.Sales.Api.Application.Models;
using ElGuerre.Microservices.Sales.Api.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Application.Queries
{
	public class OrdersByIdQueryHandler : IRequestHandler<OrdersByIdQuery, Order>
	{
		private readonly ILogger _logger;
		private readonly IOrdersRepository _repository;

		public OrdersByIdQueryHandler(ILogger<OrdersByIdQueryHandler> logger, IOrdersRepository repository)
		{
			_logger = logger;
			_repository = repository;
		}

		public async Task<Order> Handle(OrdersByIdQuery request, CancellationToken cancellationToken)
		{
			var order = await _repository.GetOrderById(request.OrderId);
			
			// Todo: Use Automapper if it's needed
			return new Order() { OrderId = order.Id, Name = order.Name };
		}
	}
}
