using ElGuerre.Microservices.Messages.Orders;
using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Application.IntegrationHandlers
{
	public class OrderToPayConsumer : IConsumer<OrderToPayMessage>
	{
		private readonly IMediator _mediator;
		private readonly IOrderRepository _repository;

		//public OrderToPayConsumer(IMediator mediator, IOrderRepository repository)
		//{
		//	_mediator = mediator;
		//	_repository = repository;
		//}

		public Task Consume(ConsumeContext<OrderToPayMessage> context)
		{

			return Task.CompletedTask;
		}
	}
}
