using System;

namespace ElGuerre.Microservices.Messages
{
	public class OrderBilledSuccessfully
	{
		public Guid CorrelationId { get; set; }
		public int OrderId { get; set; }
		public decimal Amount { get; set; }
	}
}
