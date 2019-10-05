using Automatonymous;
using ElGuerre.Microservices.Messages;
using MassTransit;
using MassTransit.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Application.Sagas
{
	//
	// https://masstransit-project.com/MassTransit/advanced/sagas/automatonymous.html
	//
	public class SalesStateMachine : MassTransitStateMachine<SalesState> , ISaga
	{
		public Guid CorrelationId { get; set; }
		public State Billed { get; private set; }
		public Event<OrderPlaced> OrderPlacedEvent { get; private set; }		
		public Event<OrderBilledSuccessfully> OrderBilledSuccessfullyEvent { get; private set; }

		public SalesStateMachine()
		{
			InstanceState(x => x.CurrentState);
			
			///
			// Event(() => OrderBilledSuccessfully, x => { x.CorrelateById(order => order.CorrelationId), context => context.Message.CorrelationId); });

			//
			// Query-based saga correlation is not available when using the MessageSession-based saga repository. 
			// Using Azure Service Bus MessageSession-based is needed, so, no query have to use to define Events. If not so NotImplementedException throw.
			//
			Event(() => OrderPlacedEvent,
			x =>
			{
				x.CorrelateById(context => context.Message.CorrelationId);
				x.InsertOnInitial = true;
				x.SetSagaFactory(ctx => new SalesState
				{
					CorrelationId = ctx.ConversationId ?? NewId.NextGuid()
				});
				x.SelectId(context => context.Message.CorrelationId);

			});
			
			Event(() => OrderBilledSuccessfullyEvent, x => x.CorrelateById(context => context.Message.CorrelationId));

			Initially(
				When(OrderPlacedEvent)
					.Then(context =>
					{
						// context.Instance.Name = context.Data.Name;
						Console.WriteLine("Initially");
					})
					.ThenAsync(async context =>
					{
						Console.WriteLine($"Order Placed Successfully: {context.Data.OrderId} to {context.Instance.CorrelationId}");
						await Task.CompletedTask;
					})
					// .TransitionTo(Ordered)					
					.TransitionTo(Billed)
					.Publish(context => new OrderBilled() { OrderId = context.Instance.OrderId, Amount = context.Instance.Amount, CorrelationId = context.Instance.CorrelationId })
										
				);

			During(Billed,
				When(OrderBilledSuccessfullyEvent) // Order to Bill ! Billing Services is listening to apply Bill -> Billed !
				.ThenAsync(async context =>
				{
					// Billed Services published everything was OK, so payed/billed was done !
					Console.WriteLine("Order Billed Successfully. The Pay OK ! !!!");
					await Task.CompletedTask;
				})
				.Finalize()
				);

			SetCompletedWhenFinalized();
		}
	}
}
