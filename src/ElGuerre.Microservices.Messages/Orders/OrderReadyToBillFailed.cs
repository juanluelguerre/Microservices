using System;
using System.Collections.Generic;
using System.Text;

namespace ElGuerre.Microservices.Messages.Orders
{
	public class OrderReadyToBillFailed
	{
		public int OrderId { get; private set; }
		public string ErrorMessage { get; private set; }

		public OrderReadyToBillFailed(int orderId, string errorMessage)
		{
			OrderId = orderId;
			ErrorMessage = errorMessage;
		}
	}
}
