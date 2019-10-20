using ElGuerre.Microservices.Ordering.Api.Application.Models;
using ElGuerre.Microservices.Ordering.Api.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Application.Queries
{
	public class OrderByIdQuery : IRequest<OrderModel>
	{
		public int OrderId { get; private set; }

		public OrderByIdQuery(int orderId)
		{
			OrderId = orderId;
		}
	}
}
