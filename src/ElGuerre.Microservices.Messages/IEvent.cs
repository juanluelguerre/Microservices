using System;

namespace ElGuerre.Microservices.Messages
{
	public interface IEvent
	{
		Guid CorrelationId { get; set; }
	}
}
