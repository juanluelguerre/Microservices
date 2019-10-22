using ElGuerre.Microservices.Ordering.Api.Application.Extensions;
using ElGuerre.Microservices.Ordering.Api.Application.Extensions;
using ElGuerre.Microservices.Ordering.Api.Application.Models;
using ElGuerre.Microservices.Ordering.Api.Application.ViewModels;
using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Customers;
using ElGuerre.Microservices.Ordering.Api.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Application.Queries
{
	public class OrderByIdQueryHandler : IRequestHandler<OrderByIdQuery, OrderModel>
	{
		private readonly ILogger _logger;
		private readonly IOrderQuery _orderQuery;

		public OrderByIdQueryHandler(ILogger<OrderByIdQueryHandler> logger, IOrderQuery orderQuery)
		{
			_logger = logger;
			_orderQuery = orderQuery;
		}

		public async Task<OrderModel> Handle(OrderByIdQuery request, CancellationToken cancellationToken)
		{
			var entityOrder = await _orderQuery.FindByIdAsync(request.OrderId);
			return entityOrder.ToOrderModel();
		}
	}
}
