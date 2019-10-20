using System;
using System.Collections.Generic;
using System.Text;

namespace ElGuerre.Microservices.Messages.Orders
{
	public class OrderToPayMessage
	{
		public int OrderId { get; private set; }
		public decimal Amount { get; private set; }

		public OrderToPayMessage(int orderId, decimal amount)
		{
			OrderId = orderId;
			Amount = amount;
		}
	}
}
