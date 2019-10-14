using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Billing.Api.Domain.Exceptions
{
	/// <summary>
	/// Exception type for domain exceptions
	/// </summary>
	public class BillingException : Exception
	{
		public BillingException()
		{ }

		public BillingException(string message)
			: base(message)
		{ }

		public BillingException(string message, Exception innerException)
			: base(message, innerException)
		{ }
	}
}
