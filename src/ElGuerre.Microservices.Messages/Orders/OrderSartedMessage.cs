using System;

namespace ElGuerre.Microservices.Messages.Orders
{
	public class OrderSartedMessage : IEvent
	{
		public Guid CorrelationId { get; set; }
		public int OrderId { get; private set; }

		public OrderSartedMessage(int orderId)
		{
			OrderId = orderId;
		}

	}
}
