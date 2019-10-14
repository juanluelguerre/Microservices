using System;

namespace ElGuerre.Microservices.Messages
{
	public class OrderNameUpdated
	{
		public Guid CorrelationId { get; set; }
		public int OrderId { get; set; }
		public string Name { get; set; }
	}
}
