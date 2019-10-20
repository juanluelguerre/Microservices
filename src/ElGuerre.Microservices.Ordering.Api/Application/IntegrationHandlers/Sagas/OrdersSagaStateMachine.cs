using Automatonymous;
using ElGuerre.Microservices.Messages;
using ElGuerre.Microservices.Messages.Orders;
using ElGuerre.Microservices.Ordering.Api.Application.Commands;
using MassTransit;
using MassTransit.Saga;
using MediatR;
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
	public class OrdersSagaStateMachine : MassTransitStateMachine<OrderSagaState>, ISaga
	{
		private readonly ILogger _logger;
		private readonly IMediator _mediator;
		public Guid CorrelationId { get; set; }

		public State Failed { get; private set; }
		public State Billed { get; private set; }
		public Event<OrderBillSuccededMessage> OrderBilledSuccededEvent { get; private set; }

		public OrdersSagaStateMachine(ILogger<OrdersSagaStateMachine> logger, IMediator mediator)
		{
			_mediator = mediator;
			_logger = logger;

			InstanceState(x => x.CurrentState);

			DefineStates();
		}

		private void DefineStates()
		{
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


					// var command = new OrderSetToBilledCommand(context.Data.OrderId);
					// await _mediator.Send(command);


					context.Instance.Name = "This is an updated Tests to know how udate Doamin using Repository or MediatR !!!!! ";
					context.Instance.Amount = 99999;


					_logger.LogInformation("Order Billed. The Pay was done successfully !!!");
				})
				//  .Publish(context => new OrderToPayMessage(context.Instance.OrderId, context.Instance.Amount))
				.Catch<Exception>(ex => ex.Then(context =>
					{
						_logger.LogError($"Catch handled for {ex.Event.GetType().Name} wit name: {ex.Event.Name}");
						context.Instance.CurrentState = Failed.ToString();
					}))
				.Finalize()
				);

			SetCompletedWhenFinalized();
	}
}
}
