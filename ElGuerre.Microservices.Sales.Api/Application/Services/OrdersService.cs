using ElGuerre.Microservices.Messages;
using ElGuerre.Microservices.Sales.Api.Application.Infrastructure;
using ElGuerre.Microservices.Sales.Api.Application.Models;
using ElGuerre.Microservices.Sales.Api.Application.Sagas;
using ElGuerre.Microservices.Sales.Api.Services;
using GreenPipes;
using MassTransit;
using MassTransit.Saga;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Application.Services
{
	public class OrdersService : IOrdersService
	{
		private readonly OrderContext _orderContext;
		private readonly IBus _bus;		

		public OrdersService(OrderContext context, IBus bus)
		{
			_orderContext = context;
			_bus = bus;
		}

		public async Task<Order> GetItem(int id)
		{
			//TODO: Replace to use Automapper.
			var orderEntity = _orderContext.Elementos.FirstOrDefault(e => e.Id == id);
			var order = new Order() { OrderId = orderEntity.Id, Name = orderEntity.Name };

			return await Task.FromResult(order);			
		}

		public async Task<PagedItem<Order>> GetItems(int pageIndex, int pageSize)
		{
			var orders = _orderContext.Orders;

			var totalItems = await orders
				.LongCountAsync();

			var ordersEntity = await orders
				.OrderBy(c => c.Name)
				.Skip(pageSize * pageIndex)
				.Take(pageSize)
				.ToListAsync();

			var pagedOrders = new PagedItem<Order>() { Total = totalItems };		
			foreach (var o in ordersEntity)
			{
				pagedOrders.Items.Add(new Order() { OrderId = o.Id, Name = o.Name });
			}
			return pagedOrders;
		}

		public async Task<bool> UpdateItem(Order order, bool sampleIsOk)
		{
			var @event = new OrderPlaced() { OrderId = order.OrderId, Name = order.Name };
			@event.CorrelationId = Guid.NewGuid();
			await _bus.Publish(@event);

			// var endpoint = await _bus.get(new Uri("rabbitmq://localhost/sales_queue"));
			//var orderRelationId = NewId.NextGuid();			

			//if (sampleIsOk)
			//{

			//	_sagaCoordinator.ProcessAsync(op, _sagaContext);
			//	return true;
			//}
			//else
			//{
			//	_sagaCoordinator.ProcessAsync(new Operation2(), _sagaContext);
			//	return false;
			//}

			return await Task.FromResult(true);		
		}
	}
}
