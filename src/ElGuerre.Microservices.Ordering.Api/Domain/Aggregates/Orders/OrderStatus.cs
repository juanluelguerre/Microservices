using ElGuerre.Microservices.Ordering.Api.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders
{
	public class OrderStatus
	{
		public const int Submitted = 1;
		public const int Paid = 2;
		public const int StockConfirmed = 3;
		public const int Cancelled = 4;

		public int Id { get; private set; }
		public string Name { get; private set; }

		public OrderStatus(int id, string name)
		{
			Id = id;
			Name = name;
		}
	}
}
