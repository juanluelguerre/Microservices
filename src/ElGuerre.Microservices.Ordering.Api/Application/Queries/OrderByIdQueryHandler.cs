using ElGuerre.Microservices.Ordering.Api.Application.Extensions;
using ElGuerre.Microservices.Ordering.Api.Application.Models;
using ElGuerre.Microservices.Ordering.Api.Application.ViewModels;
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
		private readonly OrderingContext _dbContext;

		public OrderByIdQueryHandler(ILogger<OrderByIdQueryHandler> logger, OrderingContext dbContext)
		{
			_logger = logger;
			_dbContext = dbContext;
		}

		public async Task<OrderModel> Handle(OrderByIdQuery request, CancellationToken cancellationToken)
		{
			var entityOrder = await _dbContext.Orders.FindAsync(request.OrderId);
			if (entityOrder != null)
			{
				await _dbContext.Entry(entityOrder)
					.Collection(a => a.OrderItems).LoadAsync();
				await _dbContext.Entry(entityOrder)
					.Reference(i => i.OrderStatus).LoadAsync();
				await _dbContext.Entry(entityOrder).Reference(a => a.Address)
					.LoadAsync();
			}

			return entityOrder.ToOrder();
		}
	}
}
