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
	// https://masstransit-project.com/MassTransit/advanced/sagas/automatonymous.html	
	public class BillingStateMachine : MassTransitStateMachine<BillingState>, ISaga
	{
		public Guid CorrelationId { get; set; }		

		public State Billed { get; private set; }		
		public Event<OrderBilled> OrderBilledEvent { get; private set; }	

		public BillingStateMachine()
		{
			InstanceState(x => x.CurrentState);

			Event(() => OrderBilledEvent,
				x =>
				{
					x.CorrelateById(context => context.Message.CorrelationId);
					x.InsertOnInitial = true;
					x.SetSagaFactory(ctx => new BillingState
					{
						CorrelationId = ctx.ConversationId ??  NewId.NextGuid(),
						OrderId = ctx.Message.OrderId,
					});
					x.SelectId(context => context.Message.CorrelationId);
				});

			// Event(() => OrderBilledSuccessfullyEvent, x => x.CorrelateById(context => context.Message.CorrelationId));			   

			Initially(
				When(OrderBilledEvent)
					.Then(context =>
					{
						Console.WriteLine("Initially");
					})
					.ThenAsync(async context =>
					{
						Console.WriteLine($"Order ready to bill: {context.Data.OrderId} to {context.Instance.CorrelationId}");
						await Task.CompletedTask;
					})
					.Publish(context => new OrderBilledSuccessfully() { OrderId = context.Instance.OrderId, Amount = context.Instance.Amount, CorrelationId = context.Instance.CorrelationId  })
					// .TransitionTo(Billed)					
					.Finalize()
			); ;

			//During(Billed,
			//	When(OrderBilledEvent)
			//		.Then(context =>
			//		{
			//			Console.Out.WriteAsync("Billed ! Next transport ready the product will ber shipped !");
			//		})
			//		//.ThenAsync(async context=>
			//		//{
			//		//	Console.WriteLine($"Order billed Successfully: {context.Data.OrderId} to {context.Instance.CorrelationId}");
			//		//})					
			//		.ThenAsync(async context =>
			//		{
			//			Console.WriteLine($"Order billed Successfully: {context.Data.OrderId} to {context.Instance.CorrelationId}");
			//		})
			//		.Publish(context => new OrderBilledSuccessfully(context.Instance.OrderId, context.Instance.Amount))
			//		.Finalize()
			//	);

			SetCompletedWhenFinalized();
		}
	}
}
