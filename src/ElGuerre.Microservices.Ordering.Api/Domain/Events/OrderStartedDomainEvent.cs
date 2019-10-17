using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Domain.Events
{
	public class OrderStartedDomainEvent : INotification
	{
		public string UserId { get; }
		public string UserName { get; }
		public int CardTypeId { get; }
		public string CardNumber { get; }		
		public string CardHolderName { get; }
		public DateTime CardExpiration { get; }
		public Order Order { get; }

		public OrderStartedDomainEvent(Order order,
									   string userId, string userName,
									   int cardTypeId, string cardNumber,
									   string cardHolderName,
									   DateTime cardExpiration)
		{
			Order = order;
			UserId = userId;
			UserName = userName;
			CardTypeId = cardTypeId;
			CardNumber = cardNumber;			
			CardHolderName = cardHolderName;
			CardExpiration = cardExpiration;
		}
	}
}
