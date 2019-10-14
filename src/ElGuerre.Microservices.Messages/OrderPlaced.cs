using System;

namespace ElGuerre.Microservices.Messages
{
	public class OrderPlaced
	{
		public Guid CorrelationId { get; set; }
		public int OrderId { get; set; }
		public string Name { get; set;  }

	}
}
