using ElGuerre.Microservices.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Customers
{
	public class CardType : Entity
	{
		public int Id { get; private set; }
		public string Name { get; private set; }

		public CardType(int id, string name)
		{
			Id = id;
			Name = name;
		}
	}
}
