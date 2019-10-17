using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Customers;
using ElGuerre.Microservices.Ordering.Api.Domain.Events;
using ElGuerre.Microservices.Ordering.Api.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Customers
{
	public class Customer : Entity, IAggregateRoot
	{
		public string Identity { get; private set; }
		public string Name { get; private set; }

		private List<PaymentMethod> _paymentMethods;
		public IEnumerable<PaymentMethod> PaymentMethods => _paymentMethods.AsReadOnly();

		protected Customer()
		{
			_paymentMethods = new List<PaymentMethod>();
		}		

		public Customer(string identity, string name) : this()
		{
			Identity = !string.IsNullOrWhiteSpace(identity) ? identity : throw new ArgumentNullException(nameof(identity));
			Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
		}

		public PaymentMethod VerifyOrAddPaymentMethod(
		   int cardTypeId, string alias, string cardNumber,
		   string cardHolderName, DateTime expiration, int orderId)
		{
			var existingPayment = _paymentMethods
				.SingleOrDefault(p => p.IsEqualTo(cardTypeId, cardNumber, expiration));

			if (existingPayment != null)
			{
				AddDomainEvent(new CustomerAndPaymentMethodVerifiedDomainEvent(this, existingPayment, orderId));

				return existingPayment;
			}

			var payment = new PaymentMethod(cardTypeId, alias, cardNumber, cardHolderName, expiration);

			_paymentMethods.Add(payment);

			AddDomainEvent(new CustomerAndPaymentMethodVerifiedDomainEvent(this, payment, orderId));

			return payment;
		}
	}
}
