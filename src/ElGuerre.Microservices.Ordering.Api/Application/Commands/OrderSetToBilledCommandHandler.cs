using ElGuerre.Microservices.Ordering.Api.Application.Extensions;
using ElGuerre.Microservices.Ordering.Api.Application.Queries;
using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Application.Commands
{
	public class OrderSetToBilledCommandHandler : IRequestHandler<OrderSetToBilledCommand, bool>
	{
		private readonly ILogger _logger;
		private readonly IOrderRepository _repository;
		private readonly IMediator _mediator;

		public OrderSetToBilledCommandHandler(ILogger<OrderSetToBilledCommandHandler> logger, IOrderRepository repository, IMediator mediator)
		{
			_logger = logger;
			_repository = repository;
			_mediator = mediator;
		}

		public async Task<bool> Handle(OrderSetToBilledCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Handle({nameof(OrderSetToBilledCommandHandler)}) -> {command}");			

			var commandQuery = new OrderByIdQuery(command.OrderId);
			var model = await _mediator.Send(commandQuery);
			var orderToUpdate = model.ToOrder();

			orderToUpdate.SetPaidStatus();
			return await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
		}
	}
}
