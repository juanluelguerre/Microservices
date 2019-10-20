using Automatonymous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Application.IntegrationHandlers.Sagas
{
	public class BillingSagaState : SagaStateMachineInstance
	{
		//public BillingState(Guid correlationId)
		//{
		//	CorrelationId = correlationId;
		//}

		public Guid CorrelationId { get; set; }		
		public string CurrentState { get; set; }
		public Guid? ExpirationId { get; set; }

		public bool IsOrdered { get; set; }
		public bool IsBilled { get; set; }

		public int OrderId { get; set; }
		// public string Name { get; set; }
		public decimal Amount { get; set; }
	}
}
