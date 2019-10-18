using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Application.Models
{
	[JsonObject("OrderItem")]
	public class OrderItemModel
	{
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public decimal UnitPrice { get; set; }
		public decimal Discount { get; set; }
		public int Units { get; set; }		
	}
}
