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

		public static OrderModel ToOrderModel(this Domain.Aggregates.Orders.Order order)
		{
			var model = new OrderModel()
			{
				OrderNumber = order.Id,
				Name = order.Name,
				Date = order.OrderDate,
				Status = order.OrderStatus.Name,
				Total = order.GetTotal(),
				City = order.Address.City,
				Country = order.Address.Country,
				Street = order.Address.Street,
				ZipCode = order.Address.ZipCode
			};

			foreach (var o in order.OrderItems)
			{
				var orderitem = new OrderItemModel
				{
					ProductName = o.GetItemProductName(),
					Units = o.GetUnits(),
					UnitPrice = o.GetUnitPrice(),
				};

				model.Total += o.GetUnits() * o.GetUnitPrice();
				model.OrderItems.Add(orderitem);
			}

			return model;
		}

		public static IEnumerable<OrderItemModel> ToOrderItems(this IEnumerable<Domain.Aggregates.Orders.OrderItem> orderItems)
		{
			foreach (var item in orderItems)
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

		public static CustomerModel ToCustomerModel(this Domain.Aggregates.Customers.Customer customer)
		{
			var model = new CustomerModel();


			// TODO: Complete mapping

			return model;
		}
	}
}
