using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates;
using ElGuerre.Microservices.Ordering.Api.Domain.Events;
using ElGuerre.Microservices.Ordering.Api.Domain.Exceptions;
using ElGuerre.Microservices.Ordering.Api.Domain.ValueObjects;
using ElGuerre.Microservices.Ordering.Api.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders
{
	public class Order : Entity, IAggregateRoot
	{
		public int Id { get; private set; }
		public string Name { get; private set; }
		public decimal Amount { get; private set; }
		public OrderStatus OrderStatus { get; private set; }		
		public DateTime OrderDate { get; private set; }
		public int? PaymentMethodId { get; private set; }
		public int? CustomerId { get; private set; }
		// Address is a Value Object pattern example persisted as EF Core 2.0 owned entity
		public Address Address { get; private set; }
		// DDD Patterns comment
		// Using a private collection field, better for DDD Aggregate's encapsulation
		// so OrderItems cannot be added from "outside the AggregateRoot" directly to the collection,
		// but only through the method OrderAggrergateRoot.AddOrderItem() which includes behaviour.
		private readonly List<OrderItem> _orderItems;
		public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

		private int _orderStatusId;
		private string _description;

		public static Order NewDraft()
		{
			var order = new Order();			
			return order;
		}

		protected Order()
		{
			_orderItems = new List<OrderItem>();			
		}

		public Order(string userId, string userName, Address address, int cardTypeId, string cardNumber,
			string cardHolderName, DateTime cardExpiration, int? customerId = null, int? paymentMethodId = null)
		{
			_orderStatusId = OrderStatus.Submitted;
			CustomerId = customerId;
			PaymentMethodId = paymentMethodId;
			OrderDate = DateTime.UtcNow;
			this.Address = address;

			// Add the OrderStarterDomainEvent to the domain events collection 
			// to be raised/dispatched when comitting changes into the Database [ After DbContext.SaveChanges() ]
			AddOrderStartedDomainEvent(userId, userName, cardTypeId, cardNumber, cardHolderName, cardExpiration);
		}

		// DDD Patterns comment
		// This Order AggregateRoot's method "AddOrderitem()" should be the only way to add Items to the Order,
		// so any behavior (discounts, etc.) and validations are controlled by the AggregateRoot 
		// in order to maintain consistency between the whole Aggregate. 
		public void AddOrderItem(int productId, string productName, decimal unitPrice, decimal discount, int units = 1)
		{
			var existingOrderForProduct = _orderItems.Where(o => o.ProductId == productId)
				.SingleOrDefault();

			if (existingOrderForProduct != null)
			{
				//if previous line exist modify it with higher discount  and units..

				if (discount > existingOrderForProduct.GetDiscount())
				{
					existingOrderForProduct.SetNewDiscount(discount);
				}

				existingOrderForProduct.AddUnits(units);
			}
			else
			{
				//add validated new order item

				var orderItem = new OrderItem(productId, productName, unitPrice, discount, units);
				_orderItems.Add(orderItem);
			}
		}

		public void SetPaymentId(int id)
		{
			PaymentMethodId = id;
		}

		public void SetCustomerId(int id)
		{
			CustomerId = id;
		}

		public void SetPaidStatus()
		{
			if (_orderStatusId == OrderStatus.StockConfirmed)
			{
				_orderStatusId = OrderStatus.Paid;
				_description = "The payment was performed at a simulated \"American Bank checking bank account ending on XX35071\"";

				AddDomainEvent(new OrderPaidDomainEvent(Id, OrderItems));
			}
		}

		public void SetCancelledStatus()
		{
			if (_orderStatusId == OrderStatus.Paid)
			{
				StatusChangeException(OrderStatus.Cancelled);
			}

			_orderStatusId = OrderStatus.Cancelled;
			_description = $"The order was cancelled.";

			AddDomainEvent(new OrderCancelledDomainEvent(this));
		}

		public decimal GetTotal()
		{
			return _orderItems.Sum(o => o.GetUnits() * o.GetUnitPrice());
		}

		private void AddOrderStartedDomainEvent(string userId, string userName, int cardTypeId,
			string cardNumber, string cardHolderName, DateTime cardExpiration)
		{
			var orderStartedDomainEvent = new OrderStartedDomainEvent(this, userId, userName, cardTypeId, cardNumber, cardHolderName, cardExpiration);
			this.AddDomainEvent(orderStartedDomainEvent);
		}

		private void StatusChangeException(int orderStatusIdToChange)
		{
			throw new OrderingException($"Is not possible to change the order status from {_orderStatusId} to {orderStatusIdToChange}.");
		}
	}
}
