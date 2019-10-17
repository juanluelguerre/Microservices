using ElGuerre.Microservices.Ordering.Api.Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Domain.Events
{
	public class OrderBilledDomainEvent : INotification
	{
		public int OrderId { get; private set; }

		public OrderBilledDomainEvent(int orderId)
		{
			OrderId = orderId > 0 ? orderId : throw new ArgumentNullException(nameof(orderId));
		}
	}
}
