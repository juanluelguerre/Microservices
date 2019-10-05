using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Shipping.Api.Domain.Exceptions
{
	/// <summary>
	/// Exception type for domain exceptions
	/// </summary>
	public class ShippingException : Exception
	{
		public ShippingException()
		{ }

		public ShippingException(string message)
			: base(message)
		{ }

		public ShippingException(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}
}
