using ElGuerre.Microservices.Messages.Orders;
using ElGuerre.Microservices.Ordering.Api.Application.Extensions;
using ElGuerre.Microservices.Ordering.Api.Application.Queries;
using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Orders.Application.IntegrationHandlers
{
	public class CheckOrderStatusConsumer : IConsumer<OrderCheckStatus>
	{
		private readonly ILogger _logger;
		readonly IOrderRepository _orderRepository;
		private readonly IMediator _mediator;

		public CheckOrderStatusConsumer(ILogger<CheckOrderStatusConsumer> logger, IOrderRepository orderRepository, IMediator mediator)
		{
			_logger = logger;
			_orderRepository = orderRepository;
			_mediator = mediator;
		}

		public async Task Consume(ConsumeContext<OrderCheckStatus> context)
		{			
			var commandQuery = new OrderByIdQuery(context.Message.OrderId);
			var model = await _mediator.Send(commandQuery);
			var order = model.ToOrder();

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
