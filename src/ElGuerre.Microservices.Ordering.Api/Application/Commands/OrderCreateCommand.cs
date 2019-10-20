using ElGuerre.Microservices.Ordering.Api.Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Application.Commands
{
	public class OrderCreateCommand : IRequest<bool>
	{
		private readonly List<OrderItemModel> _orderItems;

		public string UserId { get; private set; }
		public string UserName { get; private set; }

		public string City { get; private set; }
		public string Street { get; private set; }
		public string Country { get; private set; }
		public string ZipCode { get; private set; }
		public string CardNumber { get; private set; }
		public string CardHolderName { get; private set; }
		public DateTime CardExpiration { get; private set; }		
		public int CardTypeId { get; private set; }

		public IEnumerable<OrderItemModel> OrderItems => _orderItems;

		public OrderCreateCommand()
		{
			_orderItems = new List<OrderItemModel>();
		}

		public OrderCreateCommand(List<OrderItemModel> items, string userId, 
			string userName, string city, string street, string country, string zipcode,
			string cardNumber, string cardHolderName, DateTime cardExpiration, int cardTypeId) : this()
		{
			_orderItems = items;
			UserId = userId;
			UserName = userName;
			City = city;
			Street = street;
			Country = country;
			ZipCode = zipcode;
			CardNumber = cardNumber;
			CardHolderName = cardHolderName;
			CardExpiration = cardExpiration;			
			CardTypeId = cardTypeId;
			CardExpiration = cardExpiration;
		}
	}
}
