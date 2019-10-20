using ElGuerre.Microservices.Ordering.Api.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders
{
	public class OrderItem : Entity
	{
		// DDD Patterns comment
		// Using private fields, allowed since EF Core 1.1, is a much better encapsulation
		// aligned with DDD Aggregates and Domain Entities (Instead of properties and property collections)
		private string _productName;
		private decimal _unitPrice;
		private decimal _discount;
		private int _units;

		public int ProductId { get; private set; }

		protected OrderItem() { }

		public OrderItem(int productId, string productName, decimal unitPrice, decimal discount, int units = 1)
		{
			if (units <= 0)
			{
				throw new OrderingException("Invalid number of units");
			}

			if ((unitPrice * units) < discount)
			{
				throw new OrderingException("The total of order item is lower than applied discount");
			}

			ProductId = productId;

			_productName = productName;
			_unitPrice = unitPrice;
			_discount = discount;
			_units = units;
		}

		public decimal GetDiscount()
		{
			return _discount;
		}

		public int GetUnits()
		{
			return _units;
		}

		public decimal GetUnitPrice()
		{
			return _unitPrice;
		}

		public string GetItemProductName() => _productName;

		public void SetNewDiscount(decimal discount)
		{
			if (discount < 0)
			{
				throw new OrderingException("Discount is not valid");
			}

			_discount = discount;
		}

		public void AddUnits(int units)
		{
			if (units < 0)
			{
				throw new OrderingException("Invalid units");
			}

			_units += units;
		}
	}
}
