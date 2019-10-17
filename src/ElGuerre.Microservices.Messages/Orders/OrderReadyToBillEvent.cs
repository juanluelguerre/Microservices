using System;

namespace ElGuerre.Microservices.Messages
{
	public class OrderReadyToBillMessage : IEvent // ICommand<bool>
	{
		public Guid CorrelationId { get; set; }
		public int OrderId { get; private set; }		

		public OrderReadyToBillMessage(int orderId)
		{
			OrderId = orderId;
		}
	}
}
