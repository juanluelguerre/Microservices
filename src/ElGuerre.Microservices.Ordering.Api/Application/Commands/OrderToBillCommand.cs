using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Application.Commands
{
	public class OrderToBillCommand : IRequest<bool>
	{
		public int OrderId { get; private set; }

		public OrderToBillCommand(int orderId)
		{
			OrderId = orderId;
		}
	}
}
