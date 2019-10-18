using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Application.Models
{
	[JsonObject("Order")]
	public class OrderModel
	{
		public string UserId { get; }
		public string UserName { get; }

		public int OrderNumber { get; set; }
		public DateTime Date { get; set; }
		public string Status { get; set; }
		public string Name { get; set; }
		public string Street { get; set; }
		public string City { get; set; }
		public string ZipCode { get; set; }
		public string Country { get; set; }
		public List<OrderItemModel> OrderItems { get; set; }
		public decimal Total { get; set; }

		public string CardNumber { get; set; }
		public string CardHolderName { get; set; }
		public DateTime CardExpiration { get; set; }
		public string CardSecurityNumber { get; set; }
		public int CardTypeId { get; set; }
		public string Customer { get; set; }
	}
}
