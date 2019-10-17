using System;

namespace ElGuerre.Microservices.Messages
{
	public interface ICommand<T>
	{
		Guid CorrelationId { get; set; }
	}
}
