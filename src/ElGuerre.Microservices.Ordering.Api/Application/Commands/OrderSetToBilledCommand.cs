using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Application.Commands
{
	public class OrderSetToBilledCommand : IRequest<bool>
	{
		public int OrderId { get; private set; }

		public OrderSetToBilledCommand(int orderId) => OrderId = orderId;
	}
}
