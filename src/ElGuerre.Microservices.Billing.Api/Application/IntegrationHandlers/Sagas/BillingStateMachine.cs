using Automatonymous;
using ElGuerre.Microservices.Billing.Api.Application.Commands;
using ElGuerre.Microservices.Billing.Api.Domain.Exceptions;
using ElGuerre.Microservices.Messages;
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
	public class BillingStateMachine : MassTransitStateMachine<BillingState>, ISaga
	{
		private readonly ILogger _logger;
		// private readonly IMediator _mediator;

		public Guid CorrelationId { get; set; }
		public State Billed { get; private set; }
		public Event<OrderReadyToBillMessage> OrderReadyToBillEvent { get; private set; }

		public BillingStateMachine(ILoggerFactory loggerFactory /*, IMediator mediator */)
		{
			_logger = loggerFactory.CreateLogger<BillingStateMachine>();
			// _mediator = mediator;

			InstanceState(x => x.CurrentState);

			Event(() => OrderReadyToBillEvent,
				x =>
				{
					x.CorrelateById(context => context.Message.CorrelationId);
					x.InsertOnInitial = true;
					x.SetSagaFactory(ctx => new BillingState
					{
						CorrelationId = ctx.ConversationId ?? NewId.NextGuid(),
						OrderId = ctx.Message.OrderId,
					});
					x.SelectId(context => context.Message.CorrelationId);
				});

			// Event(() => OrderBilledSuccessfullyEvent, x => x.CorrelateById(context => context.Message.CorrelationId));			   

			Initially(
				When(OrderReadyToBillEvent)
					.Then(context =>
					{
						_logger.LogInformation("Initially");
					})
					.ThenAsync(async context =>
					{
						_logger.LogInformation($"Order ready to bill: {context.Data.OrderId} to {context.Instance.CorrelationId}");

						//TODO: Simulate 
						// var command = new OrderReadyToBillCommand(context.Instance.OrderId);
						// var result = await _mediator.Send(command);
						// await Task.CompletedTask;

						//throw new BillingException();						
					})
					.Publish(context => new OrderBillSuccededMessage(context.Instance.OrderId) { CorrelationId = context.Instance.CorrelationId })
					//.Catch<Exception>(ex =>
					//{
					//	// TODO: Treat exceptions

					//	//TODO: Publish AbortedEvent !!!!!

					//	return rew BillingException();
					//})

					.Finalize()
			);

			//During(Billed,
			//	When(OrderBilledEvent)
			//		.Then(context =>
			//		{
			//			_logger.LogInformation("Billed ! Next transport ready the product will ber shipped !");
			//		})
			//		//.ThenAsync(async context=>
			//		//{
			//		//	_logger.LogInformation($"Order billed Successfully: {context.Data.OrderId} to {context.Instance.CorrelationId}");
			//		//})					
			//		.ThenAsync(async context =>
			//		{
			//			_logger.LogInformation($"Order billed Successfully: {context.Data.OrderId} to {context.Instance.CorrelationId}");
			//		})
			//		.Publish(context => new OrderBilledSuccessfully(context.Instance.OrderId, context.Instance.Amount))
			//		.Finalize()
			//	);

			SetCompletedWhenFinalized();
		}

	}
}
