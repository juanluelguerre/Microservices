using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Domain.Exceptions
{
	/// <summary>
	/// Exception type for domain exceptions
	/// </summary>
	public class OrderingException : Exception
	{
		public OrderingException()
		{ }

		public OrderingException(string message)
			: base(message)
		{ }

		public OrderingException(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}
}
