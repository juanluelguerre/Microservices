using ElGuerre.Microservices.Sales.Api.Domain.Events;
using ElGuerre.Microservices.Sales.Api.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Application.Commands
{
	public class OrderUpdateNameCommandHandler : IRequestHandler<OrderUpdateNameCommand, bool>
	{
		private readonly ILogger _logger;
		private readonly IMediator _mediator;
		private readonly IOrdersRepository _repository;

		public OrderUpdateNameCommandHandler(IMediator mediator, ILogger<OrderUpdateNameCommandHandler> logger)
		{
			_mediator = mediator;
			_logger = logger;
		}

		public async Task<bool> Handle(OrderUpdateNameCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Handle({nameof(OrderUpdateNameCommandHandler)}) -> {command}");

			var updated = await _repository.UpdateName(command.OrderId, command.Name);
			if (updated)
			{
				//  await _mediator.Publish(Apply(command));
				return updated;
			}
			return false;
		}

		private OrderNameUpdated Apply(OrderUpdateNameCommand command)
		{
			if (command == null)
			{
				throw new System.ArgumentNullException(nameof(command));
			}

			return new OrderNameUpdated(command.OrderId, command.Name);
		}
	}
}
