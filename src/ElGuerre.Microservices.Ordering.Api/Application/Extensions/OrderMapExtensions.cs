using ElGuerre.Microservices.Ordering.Api.Application.Models;
using ElGuerre.Microservices.Ordering.Api.Application.ViewModels;
using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders;
using ElGuerre.Microservices.Ordering.Api.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Application.Extensions
{
	public static class OrderMapExtensions
	{
		private static object entityOrder;

		public static OrderModel ToOrder(this Domain.Aggregates.Orders.Order entityOrder)
		{
			var order = new OrderModel()
			{
				OrderNumber = entityOrder.Id,
				Name = entityOrder.Name,
				Date = entityOrder.OrderDate,
				Status = entityOrder.OrderStatus.Name,
				Total = entityOrder.GetTotal(),
				City = entityOrder.Address.City,
				Country = entityOrder.Address.Country,
				Street = entityOrder.Address.Street,
				ZipCode = entityOrder.Address.ZipCode
			};

			foreach (var o in entityOrder.OrderItems)
			{
				var orderitem = new OrderItemModel
				{
					ProductName = o.GetItemProductName(),
					Units = o.GetUnits(),
					UnitPrice = o.GetUnitPrice(),
				};

				order.Total += o.GetUnits() * o.GetUnitPrice();
				order.OrderItems.Add(orderitem);
			}

			return order;
		}

		public static IEnumerable<OrderItemModel> ToOrderItems(this IEnumerable<Domain.Aggregates.Orders.OrderItem> entityOrderItems)
		{
			foreach (var item in entityOrderItems)
			{
				yield return item.ToOrderItem();
			}
		}

		public static OrderItemModel ToOrderItem(this Domain.Aggregates.Orders.OrderItem item)
		{
			return new OrderItemModel()
			{
				ProductId = item.ProductId,
				ProductName = item.GetItemProductName(),
				UnitPrice = item.GetUnitPrice(),
				Units = item.GetUnits(),
				Discount = item.GetDiscount()
			};
		}

		public static Order ToOrder(this OrderModel model)
		{
			var address = new Address(model.Street, model.City, model.Country, model.ZipCode);
			var order = new Order(model.UserId, model.UserName, address, model.CardTypeId, model.CardNumber, model.CardHolderName, model.CardExpiration);

			foreach (var o in model.OrderItems)
			{				
				order.AddOrderItem(o.ProductId, o.ProductName, o.UnitPrice, o.Discount, o.Units);
			}

			return order;
		}
	}
}
