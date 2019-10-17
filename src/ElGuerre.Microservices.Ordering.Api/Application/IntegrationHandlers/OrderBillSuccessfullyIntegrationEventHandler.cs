//using ElGuerre.Microservices.Messages;
//using ElGuerre.Microservices.Ordering.Api.Application.Commands;
//using ElGuerre.Microservices.Ordering.Api.Application.Models;
//using ElGuerre.Microservices.Ordering.Api.Domain.Orders;
//using MassTransit;
//using MediatR;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ElGuerre.Microservices.Ordering.Api.Application.IntegrationEvents.EventHanders
//{
//	public class OrderSetToBilledCommandHandler : IConsumer<OrderBillSuccededMessage>
//	{
//		private readonly ILogger _logger;
//		private readonly IMediator _mediator;
//		private readonly IOrdersRepository _repository;

//		public OrderSetToBilledCommandHandler(ILogger<OrderSetToBilledCommandHandler> logger, IMediator mediator, IOrdersRepository repository)
//		{
//			_logger = logger;
//			_mediator = mediator;
//			_repository = repository;
//		}

//		public async Task Consume(ConsumeContext<OrderBillSuccededMessage> context)
//		{			
//			_logger.LogInformation($"Order with {context.Message.OrderId} accepted.");

//			var order = await _repository.GetByIdAsync(context.Message.OrderId);
//			var command = new OrderSetToBilledCommand(order.Id);
//			await _mediator.Send(command);
//		}
//	}
//}
