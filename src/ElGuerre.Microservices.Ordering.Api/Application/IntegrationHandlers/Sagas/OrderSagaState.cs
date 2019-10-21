using Automatonymous;
using MassTransit.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Application.IntegrationHandlers.Sagas
{
	public class OrderSagaState : SagaStateMachineInstance
	{
		public Guid CorrelationId { get; set; }
		public Guid SessionId { get; set; }

		public int CurrentState { get; set; }
		//public Guid? ExpirationId { get; set; }

		//public bool IsOrdered { get; set; }		

		public int OrderId { get; set; }
		public string Name { get; set; }
		public decimal Amount { get; set; }		
	}
}
