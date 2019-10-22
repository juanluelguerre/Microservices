using ElGuerre.Microservices.Ordering.Api.Domain.Exceptions;
using ElGuerre.Microservices.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Customers
{
	public class PaymentMethod : Entity
	{
		private string _alias;
		private string _cardNumber;
		private string _securityNumber;
		private string _cardHolderName;
		private DateTime _expiration;

		private int _cardTypeId;
		public CardType CardType { get; private set; }


		protected PaymentMethod() { }

		public PaymentMethod(int cardTypeId, string alias, string cardNumber, string cardHolderName, DateTime expiration)
		{

			_cardNumber = !string.IsNullOrWhiteSpace(cardNumber) ? cardNumber : throw new OrderingException(nameof(cardNumber));			
			_cardHolderName = !string.IsNullOrWhiteSpace(cardHolderName) ? cardHolderName : throw new OrderingException(nameof(cardHolderName));

			if (expiration < DateTime.UtcNow)
			{
				throw new OrderingException(nameof(expiration));
			}

			_alias = alias;
			_expiration = expiration;
			_cardTypeId = cardTypeId;
		}

		public bool IsEqualTo(int cardTypeId, string cardNumber, DateTime expiration)
		{
			return _cardTypeId == cardTypeId
				&& _cardNumber == cardNumber
				&& _expiration == expiration;
		}
	}
}
