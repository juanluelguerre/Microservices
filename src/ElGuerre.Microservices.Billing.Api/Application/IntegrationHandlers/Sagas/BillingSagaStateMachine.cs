using Automatonymous;
using ElGuerre.Microservices.Billing.Api.Application.Commands;
using ElGuerre.Microservices.Billing.Api.Domain.Exceptions;
using ElGuerre.Microservices.Messages;
using ElGuerre.Microservices.Messages.Orders;
using ElGuerre.Microservices.Shared.Infrastructure;
using MassTransit;
using MassTransit.Saga;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Application.IntegrationHandlers.Sagas
{
	// https://masstransit-project.com/MassTransit/advanced/sagas/automatonymous.html	
	public class BillingSagaStateMachine : MassTransitStateMachine<BillingSagaState>, ISaga
	{
		private readonly ILogger _logger;
		private readonly IMediator _mediator;

		public Guid CorrelationId { get; set; }
		public State Failed { get; private set; }
		public State ReadyToBill { get; private set; }
		public State Billed { get; private set; }

		public Event<OrderReadyToBillMessage> OrderReadyToBillEvent { get; private set; }
		public Event<Fault<OrderReadyToBillMessage>> OrderReadyToBillFaulted { get; private set; }
		public Event<OrderBillSuccededMessage> OrderBilledEvent { get; private set; }
		
		public BillingSagaStateMachine(ILogger<BillingSagaStateMachine> logger, IMediator mediator)
		{
			_logger = logger;
			_mediator = mediator;

			InstanceState(x => x.CurrentState, ReadyToBill, Billed, Failed);

			DefineStates();
		}


		private void DefineStates()
		{
			Event(() => OrderReadyToBillEvent,
					x =>
					{
						x.CorrelateById(context => context.Message.CorrelationId);
						x.InsertOnInitial = true;
						x.SetSagaFactory(ctx => new BillingSagaState
						{
							CorrelationId = ctx.ConversationId ?? NewId.NextGuid(),
							OrderId = ctx.Message.OrderId,
						});
						x.SelectId(context => context.Message.CorrelationId);
					});
			Event(() => OrderReadyToBillFaulted, x => x.CorrelateById(context => context.Message.Message.CorrelationId));
				
			Event(() => OrderBilledEvent, x => x.CorrelateById(context => context.Message.CorrelationId));			   

			Initially(
				When(OrderReadyToBillEvent)
					.Then(context =>
					{
						_logger.LogInformation("Initially");
					})
					.ThenAsync(async context =>
					{
						_logger.LogInformation($"Order ready to bill: {context.Data.OrderId} to {context.Instance.CorrelationId}");
						await Task.CompletedTask;

						//TODO: Simulate 
						// var command = new OrderReadyToBillCommand(context.Instance.OrderId);
						// var result = await _mediator.Send(command);						
						// if (result) { }

						// Force an exception
						//throw new BillingException();						
					})
					.Publish(context => new OrderBillSuccededMessage(context.Instance.OrderId) { CorrelationId = context.Instance.CorrelationId })
					.TransitionTo(Billed)
					// 
					// https://stackoverflow.com/questions/47267504/finalizing-saga-on-exception
					//
					.Catch<Exception>(ex =>
						ex.Then(context =>
						{
							_logger.LogError($"Catch handled for {ex.Event.GetType().Name} wit name: {ex.Event.Name}");
						})
						.TransitionTo(Failed))					
				);
			
			During(Billed,
				When(OrderBilledEvent)
					// .Publish(context => new OrderBillSuccededMessage(context.Instance.OrderId) { CorrelationId = context.Instance.CorrelationId })
					.Finalize()
				);

			During(Failed,
				When(OrderReadyToBillFaulted)
					.Then(context =>
					{
						_logger.LogInformation("Error to Bill !!!");
					})
					.Publish(context => new OrderReadyToBillFailed(context.Instance.OrderId, ""))
					.Finalize()
				);

			SetCompletedWhenFinalized();
		}
	}
}
