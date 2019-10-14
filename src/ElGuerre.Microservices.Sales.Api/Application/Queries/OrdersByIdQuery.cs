using ElGuerre.Microservices.Sales.Api.Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Application.Queries
{
	public class OrdersByIdQuery : IRequest<Order>
	{
		public int OrderId { get; private set; }

		public OrdersByIdQuery(int orderId)
		{
			OrderId = orderId;
		}
	}
}
