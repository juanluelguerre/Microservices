using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Domain.Exceptions
{
	/// <summary>
	/// Exception type for domain exceptions
	/// </summary>
	public class OrderException : Exception
	{
		public OrderException()
		{ }

		public OrderException(string message)
			: base(message)
		{ }

		public OrderException(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}
}
