//using Automatonymous;
//using ElGuerre.Microservices.Messages;
//using MassTransit;
//using MassTransit.Saga;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ElGuerre.Microservices.Sales.Api.Application.Sagas
//{
//	//
//	// https://masstransit-project.com/MassTransit/advanced/sagas/automatonymous.html
//	//
//	public class SalesStateMachine : MassTransitStateMachine<SalesState> , ISaga
//	{
//		public Guid CorrelationId { get; set; }

//		public State Active { get; private set; }
//		public State Billed { get; private set; }
//		public Event<OrderPlaced> OrderPlacedEvent { get; private set; }
//		//public Event<OrderBilledIntegratedEvent> OrderBilled { get; private set; }
//		public Event<OrderBilled> OrderBilledSuccessfully { get; private set; }


//		public SalesStateMachine()
//		{
//			InstanceState(x => x.CurrentState);

//			// https://stackoverflow.com/questions/35226656/how-can-i-correlate-events-in-a-masstransit-state-machine-without-using-a-guid
//			//Event(() => OrderPlaced,
//			//	x =>
//			//	{
//			//		x.CorrelateById(order => order.OrderId, context => context.Message.OrderId);
//			//		x.InsertOnInitial = true;
//			//		x.SetSagaFactory(ctx => new SalesState
//			//		{
//			//			CorrelationId = Guid.Parse("{24A80C6A-AAD5-4A1A-8568-CADABE894AAD}"), // NewId.NextGuid(),
//			//			OrderId = ctx.Message.OrderId,
//			//			Name = ctx.Message.Name,
//			//			Amount = 1
//			//		}); ;
//			//		x.SelectId(context => NewId.NextGuid());
//			//	});			

//			Event(() => OrderPlacedEvent,
//			x =>
//			{				
//				x.CorrelateById(order => order.OrderId, context => context.Message.OrderId);
//				x.InsertOnInitial = true;
//				x.SetSagaFactory(ctx => new SalesState
//				{
//					CorrelationId = ctx.ConversationId ?? NewId.NextGuid(), // Guid.Parse("{24A80C6A-AAD5-4A1A-8568-CADABE894AAD}"),
//					//OrderId = ctx.Message.OrderId,
//					//Name = ctx.Message.Name,
//					//Amount = 1
//				});
//				x.SelectId(context => context.Message.CorrelationId );
//				//x.OnMissingInstance(m =>
//				//{
//				//	Console.WriteLine("Something was wrong....");
//				//	return m.Fault();
//				//});
				
//			});
//			Event(() => OrderBilledSuccessfully, x => { x.CorrelateById(order => order.OrderId, context => context.Message.OrderId); });


//			Initially(
//				When(OrderPlacedEvent)
//					.Then(context =>
//					{
//						// context.Instance.Name = context.Data.Name;
//						Console.WriteLine("Initially");
//					})
//					.ThenAsync(async context =>
//					{
//						Console.WriteLine($"Order Placed Successfully: {context.Data.OrderId} to {context.Instance.CorrelationId}");
//						await Task.CompletedTask;
//					})
//					// .TransitionTo(Ordered)					
//					.Publish(context => new OrderBilled(context.Instance.OrderId, context.Instance.Amount))
//					.TransitionTo(Billed)					
//				);

//			During(Billed,
//				When(OrderBilledSuccessfully) // Order to Bill ! Billing Services is listening to apply Bill -> Billed !
//				.ThenAsync(async context =>
//				{
//					// Billed Services published everything was OK, so payed/billed was done !
//					Console.WriteLine("Order Billed Successfully. The Pay OK ! !!!");
//					await Task.CompletedTask;
//				})
//				.Finalize()
//				);

//			SetCompletedWhenFinalized();
//		}

//		//class OrderPlacedEvent : OrderPlacedIntegratedEvent
//		//{
//		//	readonly OrderPlacedIntegratedEvent _instance;

//		//	public OrderPlacedEvent(OrderPlacedIntegratedEvent instance)
//		//	{
//		//		_instance = instance;
//		//	}

//		//	// public Guid EventId => _instance.EventId;
//		//	public Guid CorrelationId { get; set; }
//		//}

//	}
//}
