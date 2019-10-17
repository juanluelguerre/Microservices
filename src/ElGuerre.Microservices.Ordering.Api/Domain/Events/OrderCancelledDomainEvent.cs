using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Domain.Events
{
	public class OrderCancelledDomainEvent : INotification
	{
		public Order Order { get; }

		public OrderCancelledDomainEvent(Order order)
		{
			Order = order;
		}
	}
}
