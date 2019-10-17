using ElGuerre.Microservices.Messages.Orders;
using ElGuerre.Microservices.Ordering.Api.Domain.Orders;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Orders.Application.IntegrationHandlers
{
	public class CheckOrderStatusConsumer : IConsumer<OrderCheckStatus>
	{
		readonly IOrdersRepository _orderRepository;

		public CheckOrderStatusConsumer(IOrdersRepository orderRepository)
		{
			_orderRepository = orderRepository;
		}

		public async Task Consume(ConsumeContext<OrderCheckStatus> context)
		{
			var order = await _orderRepository.GetByIdAsync(context.Message.OrderId);
			if (order == null)
				throw new InvalidOperationException("Order not found");

			await context.RespondAsync<OrderCheckStatusResult>(new
			{
				OrderId = order.Id,
				order.Name
			});
		}
	}
}
