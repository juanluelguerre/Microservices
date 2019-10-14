using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Application.Commands
{
	public class OrdersUpdateNameCommand : IRequest
	{
		public int OrderId { get; private set; }
		public string Name { get; private set; }

		public OrdersUpdateNameCommand(int orderId, string name)
		{
			// TODO: Review
			// OrderId = orderId > 0 ? orderId : throw new ArgumentNullException(nameof(orderId));

			OrderId = orderId >= 0 ? orderId : throw new ArgumentNullException(nameof(orderId));
			Name = name ?? throw new ArgumentNullException(nameof(name));
		}
	}
}
