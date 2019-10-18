using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Billing.Api.Application.Commands
{
	public class OrderReadyToBillCommand : IRequest<bool>
	{
		public int OrderId { get; private set; }

		public OrderReadyToBillCommand(int orderId)
		{
			OrderId = orderId;
		}
	}
}
