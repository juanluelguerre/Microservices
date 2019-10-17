using ElGuerre.Microservices.Ordering.Api.Application.Models;
using ElGuerre.Microservices.Ordering.Api.Domain.Orders;
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
	public class OrdersPagedQueryHandler : IRequestHandler<OrdersPagedQuery, PagedItems<Order>>
	{
		private readonly ILogger _logger;
		private readonly IOrdersRepository _repository;

		public OrdersPagedQueryHandler(ILogger<OrdersPagedQueryHandler> logger, IOrdersRepository repository)
		{
			_logger = logger;
			_repository = repository;
		}

		public async Task<PagedItems<Order>> Handle(OrdersPagedQuery request, CancellationToken cancellationToken)
		{
			var orders = await _repository.GetAsync(request.PageIndex, request.PageSize);

			var totalItems = await orders
				.LongCountAsync();

			var ordersEntity = await orders
				.OrderBy(c => c.Name)
				.Skip(request.PageSize * (request.PageIndex-1))
				.Take(request.PageSize)
				.ToListAsync();


			var pagedOrders = new PagedItems<Order>();
			pagedOrders.PageIndex = request.PageIndex;
			pagedOrders.PageSize = request.PageSize;
			pagedOrders.TotalItems = totalItems;

			foreach (var o in ordersEntity)
			{
				pagedOrders.Items.Add(new Order() { OrderId = o.Id, Name = o.Name });
			}

			return pagedOrders;
		}
	}
}
