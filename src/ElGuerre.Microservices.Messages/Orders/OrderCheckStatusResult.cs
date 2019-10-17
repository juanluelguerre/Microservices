using System;
using System.Collections.Generic;
using System.Text;

namespace ElGuerre.Microservices.Messages.Orders
{
	public class OrderCheckStatusResult
	{
		public int OrderId { get; set; }
		public string Name {get; set;}
	}
}
