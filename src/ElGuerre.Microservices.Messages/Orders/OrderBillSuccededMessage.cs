using System;

namespace ElGuerre.Microservices.Messages
{
	public class OrderBillSuccededMessage :  IEvent
	{
		public Guid CorrelationId { get; set; }
		public int OrderId { get; private set; }		

		public OrderBillSuccededMessage(int orderId)
		{
			OrderId = orderId;
		}
	}
}
