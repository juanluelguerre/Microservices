using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api
{
	[JsonObject("Ordering")]
	public class OrderingSettings
	{
		public string EventBusUrl { get; set; }
		public string EventBusKeyName { get; set; }
		public string EventBusSharedAccessKey { get; set; }
		public string EventBusConnectionString { get; set; }
		public string OrdersDBConnectionString { get; set; }
	}
}
