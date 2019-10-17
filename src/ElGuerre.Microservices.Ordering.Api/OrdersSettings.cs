using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api
{
	public class OrdersSettings
	{
		public string EventBusConnection { get; set; }
		public string OrdersDBConnection { get; set; }
	}
}
