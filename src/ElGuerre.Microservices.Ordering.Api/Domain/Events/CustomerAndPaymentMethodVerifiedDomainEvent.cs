using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Customers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Domain.Events
{
	public class CustomerAndPaymentMethodVerifiedDomainEvent : INotification
	{
		public Customer Customer { get; private set; }
		public PaymentMethod Payment { get; private set; }
		public int OrderId { get; private set; }

		public CustomerAndPaymentMethodVerifiedDomainEvent(Customer customer, PaymentMethod payment, int orderId)
		{
			Customer = customer;
			Payment = payment;
			OrderId = orderId;
		}
	}
}
