using ElGuerre.Microservices.Messages;
using ElGuerre.Microservices.Sales.Api.Application.Commands;
using ElGuerre.Microservices.Sales.Api.Application.Infrastructure;
using ElGuerre.Microservices.Sales.Api.Application.Models;
using ElGuerre.Microservices.Sales.Api.Application.Queries;
using ElGuerre.Microservices.Sales.Api.Application.Sagas;
using ElGuerre.Microservices.Sales.Api.Services;
using GreenPipes;
using MassTransit;
using MassTransit.Saga;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Application.Services
{
	public class OrdersService : IOrdersService
	{
		private readonly ILogger _logger;
		private readonly IMediator _mediatr;
		// private readonly IBus _bus;

		public OrdersService(ILogger<OrdersService> logger, IMediator mediatr/*, IBus bus*/)
		{
			_logger = logger;
			_mediatr = mediatr;
			// _bus = bus;
		}

		public async Task<Order> GetOrder(int orderId)
		{
			var query = new OrdersByIdQuery(orderId);
			return await _mediatr.Send(query);;
		}

		public async Task<PagedItems<Order>> GetOrders(int pageIndex, int pageSize)
		{
			var query = new OrdersPagedQuery(pageIndex, pageSize);
			return await _mediatr.Send(query);
		}

		public async Task<bool> UpdateOrderName(int orderId, string name)
		{
			var command = new OrdersUpdateNameCommand(orderId, name);
			await _mediatr.Send(command);

			return true;
		}

		//public async Task<bool> UpdateItem(Order order, bool sampleIsOk)
		//{

		//	if (await UpdateNameItem(order.OrderId, order.Name))
		//	{
		//		//
		//		// Integration Services Operation
		//		//
		//		var @event = new OrderPlaced() { OrderId = order.OrderId, Name = order.Name };
		//		@event.CorrelationId = Guid.NewGuid();
		//		await _bus.Publish(@event);

		//		return true;
		//	}
		//	return false;
		//}
	}
}
