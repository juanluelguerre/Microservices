//using ElGuerre.Microservices.Messages;
//using ElGuerre.Microservices.Ordering.Api.Application.Models;
//using ElGuerre.Microservices.Shared.Infrastructure;
//using MassTransit;
//using MediatR;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ElGuerre.Microservices.Ordering.Api.Application.IntegrationEvents.EventHanders
//{
//	public class OrderReadyToBillIntegrationEventHandler : IConsumer<OrderReadyToBillMessage>
//	{
//		private readonly ILogger _logger;
//		private readonly IMediator _mediator;
//		private readonly IIntegrationService _service;


//		public OrderReadyToBillIntegrationEventHandler(ILogger<OrderReadyToBillIntegrationEventHandler> logger, IMediator mediator, IIntegrationService service)
//		{
//			_logger = logger;
//			_mediator = mediator;
//			_service = service;
//		}

//		public async Task Consume(ConsumeContext<OrderReadyToBillMessage> context)
//		{			
//			//var sessionId = context.SessionId();

//			_logger.LogInformation($"Order with {context.Message.OrderId} accepted.");


//			var command = new OrderReadyToBillMessage(context.Message.OrderId);
//			var result = await _service.SendToEventBusAsync(command, Program.AppName);
			




//		}
//	}
//}
