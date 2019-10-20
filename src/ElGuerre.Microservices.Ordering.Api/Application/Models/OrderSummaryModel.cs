using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Application.Models
{
	[JsonObject("OrderSummary")]
	public class OrderSummaryModel
	{
		public int OrderNumber { get; set; }
		public DateTime Date { get; set; }
		public string Status { get; set; }
		public double Total { get; set; }
	}
}
