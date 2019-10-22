using ElGuerre.Microservices.Ordering.Api.Application.Extensions;
using ElGuerre.Microservices.Ordering.Api.Application.Models;
using ElGuerre.Microservices.Ordering.Api.Application.ViewModels;
using ElGuerre.Microservices.Ordering.Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Application.Queries
{
	public class OrderPagedQueryHandler : IRequestHandler<OrderPagedQuery, PagedItemsViewModel<OrderModel>>
	{
		private readonly ILogger _logger;
		private readonly OrderingContext _dbContext;

		public OrderPagedQueryHandler(ILogger<OrderPagedQueryHandler> logger, OrderingContext dbContext)
		{
			_logger = logger;
			_dbContext = dbContext;
		}

		public async Task<PagedItemsViewModel<OrderModel>> Handle(OrderPagedQuery request, CancellationToken cancellationToken)
		{
			var totalItems = await _dbContext.Orders
				.LongCountAsync();

			var ordersEntity = await _dbContext.Orders
				.OrderBy(c => c.Name)
				.Skip(request.PageSize * (request.PageIndex - 1))
				.Take(request.PageSize)
				.ToListAsync();

			var pagedOrders = new List<OrderModel>();
			foreach (var eo in ordersEntity)
			{
				pagedOrders.Add(eo.ToOrderModel());
			}

			return new PagedItemsViewModel<OrderModel>(request.PageIndex, request.PageSize, totalItems, pagedOrders.AsEnumerable());
		}
	}
}
