using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Billing.Api
{
	[JsonObject("Billing")]
	public class BillingSettings
	{
		public string EventBusUrl { get; set; }
		public string EventBusKeyName { get; set; }
		public string EventBusSharedAccessKey { get; set; }
		public string EventBusConnectionString { get; set; }
		public string OrdersDBConnectionString { get; set; }
	}
}
