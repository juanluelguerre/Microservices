using ElGuerre.Microservices.Sales.Api.Application.Models;
using ElGuerre.Microservices.Sales.Api.Application.Queries;
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
	public class OrderUpdateNameCommandHandler :
		AsyncRequestHandler<OrdersUpdateNameCommand>
	{
		private readonly ILogger _logger;
		private readonly IMediator _mediator;
		private readonly IOrdersRepository _repository;

		public OrderUpdateNameCommandHandler(ILogger<OrderUpdateNameCommandHandler> logger, IMediator mediator, IOrdersRepository repository)
		{
			_logger = logger;
			_mediator = mediator;			
			_repository = repository;
		}

		protected override async Task Handle(OrdersUpdateNameCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Handle({nameof(OrderUpdateNameCommandHandler)}) -> {command}");

			var updated = await _repository.UpdateName(command.OrderId, command.Name);
			if (updated)
			{
				await _mediator.Publish(Apply(command));
			}
			_logger.LogWarning($"Order name {(updated ? "" : "Cannot been")} updated. Try again it latter !");
		}

		private OrderNameUpdated Apply(OrdersUpdateNameCommand command)
		{
			if (command == null)
			{
				throw new System.ArgumentNullException(nameof(command));
			}

			return new OrderNameUpdated(command.OrderId, command.Name);
		}

	}
}
