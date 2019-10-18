using Automatonymous;
using ElGuerre.Microservices.Messages;
using ElGuerre.Microservices.Messages.Orders;
using ElGuerre.Microservices.Ordering.Api.Application.Commands;
using MassTransit;
using MassTransit.Saga;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Application.IntegrationHandlers.Sagas
{
	//
	// https://masstransit-project.com/MassTransit/advanced/sagas/automatonymous.html
	//
	public class OrdersStateMachine : MassTransitStateMachine<SalesState> , ISaga
	{
		private readonly ILogger _logger;
		public Guid CorrelationId { get; set; }
		public State Billed { get; private set; }
		// public Event<OrderSartedMessage> OrderStartedEvent { get; private set; }		
		public Event<OrderBillSuccededMessage> OrderBilledSuccededEvent { get; private set; }

		public OrdersStateMachine(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<OrdersStateMachine>();

			InstanceState(x => x.CurrentState);
						
			///
			// Event(() => OrderBilledSuccessfully, x => { x.CorrelateById(order => order.CorrelationId), context => context.Message.CorrelationId); });

			//
			// Query-based saga correlation is not available when using the MessageSession-based saga repository. 
			// Using Azure Service Bus MessageSession-based is needed, so, no query have to use to define Events. If not so NotImplementedException throw.
			//
			//Event(() => OrderStartedEvent,
			//x =>
			//{
			//	x.CorrelateById(context => context.Message.CorrelationId);
			//	x.InsertOnInitial = true;
			//	x.SetSagaFactory(ctx => new SalesState
			//	{
			//		CorrelationId = ctx.ConversationId ?? NewId.NextGuid()
			//	});
			//	x.SelectId(context => context.Message.CorrelationId);

			//});
			
			Event(() => OrderBilledSuccededEvent, x => x.CorrelateById(context => context.Message.CorrelationId));

			//Initially(
			//	When(OrderStartedEvent)
			//		.Then(context =>
			//		{
			//			// context.Instance.Name = context.Data.Name;
			//			_logger.LogInformation("Initially");
			//		})
			//		.ThenAsync(async context =>
			//		{
			//			_logger.LogInformation($"Order Placed Successfully: {context.Data.OrderId} to {context.Instance.CorrelationId}");
			//			await Task.CompletedTask;
			//		})
			//		// .TransitionTo(Ordered)					
			//		.TransitionTo(Billed)
			//		.Publish(context => new OrderToBillMessage(context.Instance.OrderId) { CorrelationId = context.Instance.CorrelationId })										
			//	);

			Initially(
				When(OrderBilledSuccededEvent) // Order to Bill ! Billing Services is listening to apply Bill -> Billed !
				.ThenAsync(async context =>
				{
					// Billed Services published everything was OK, so payed/billed was done !

					//var command = new OrderSetToBilledCommand(context.Data.OrderId);
					//await _mediator.Send(command);

					_logger.LogInformation("Order Billed. The Pay was done successfully !!!");
					await Task.CompletedTask;
				})
				.Finalize()
				);

			SetCompletedWhenFinalized();
		}
	}
}
