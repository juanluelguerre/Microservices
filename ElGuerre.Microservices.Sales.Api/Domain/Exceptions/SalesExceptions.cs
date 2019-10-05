using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Domain.Exceptions
{
	/// <summary>
	/// Exception type for domain exceptions
	/// </summary>
	public class SalesException : Exception
	{
		public SalesException()
		{ }

		public SalesException(string message)
			: base(message)
		{ }

		public SalesException(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}
}
