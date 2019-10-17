using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Domain.Events
{
	public class OrderPaidDomainEvent : INotification
	{
		public int OrderId { get; }
		public IEnumerable<OrderItem> OrderItems { get; }

		public OrderPaidDomainEvent(int orderId,
			IEnumerable<OrderItem> orderItems)
		{
			OrderId = orderId;
			OrderItems = orderItems;
		}
	}
}
