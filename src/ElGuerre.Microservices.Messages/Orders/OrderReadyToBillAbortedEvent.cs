using System;
using System.Collections.Generic;
using System.Text;

namespace ElGuerre.Microservices.Messages.Orders
{
	public class OrderReadyToBillAbortedEvent : IEvent
	{
		public Guid CorrelationId { get; set; }


	}
}
