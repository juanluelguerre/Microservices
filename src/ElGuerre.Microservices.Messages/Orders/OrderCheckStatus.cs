using System;
using System.Collections.Generic;
using System.Text;

namespace ElGuerre.Microservices.Messages.Orders
{
	public class OrderCheckStatus
	{
		public int OrderId { get; private set; }

		public OrderCheckStatus(int orderId)
		{
			OrderId = orderId;
		}
	}
}
