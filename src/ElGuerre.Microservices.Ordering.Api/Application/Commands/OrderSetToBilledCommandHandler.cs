using ElGuerre.Microservices.Ordering.Api.Domain.Orders;
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
		private readonly IOrdersRepository _repository;

		public OrderSetToBilledCommandHandler(ILogger<OrderSetToBilledCommandHandler> logger, IOrdersRepository repository)
		{
			_logger = logger;
			_repository = repository;
		}

		public async Task<bool> Handle(OrderSetToBilledCommand command, CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Handle({nameof(OrderSetToBilledCommandHandler)}) -> {command}");

			var order = await _repository.GetByIdAsync(command.OrderId);
			order.SetPaidStatus();
			return await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
		}
	}
}
