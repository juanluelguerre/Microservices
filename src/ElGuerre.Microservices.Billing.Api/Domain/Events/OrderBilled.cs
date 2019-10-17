using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Billing.Api.Domain.Events
{
	public class OrderBilled : INotification
	{
		public int OrderId { get; private set; }		

		public OrderBilled(int orderId)
		{
			OrderId = orderId > 0 ? orderId : throw new ArgumentNullException(nameof(orderId));			
		}
	}
}
