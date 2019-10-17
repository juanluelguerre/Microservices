
using ElGuerre.Microservices.Messages;
using ElGuerre.Microservices.Ordering.Api.Application.Commands;
using ElGuerre.Microservices.Shared.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Application.Commands
{
	public class OrderToBillCommandHandler : IRequestHandler<OrderToBillCommand, bool>
	{
		private readonly ILogger _logger;				
		private readonly IIntegrationService _integrationService;

		public OrderToBillCommandHandler(ILogger<OrderToBillCommandHandler> logger, IIntegrationService integrationService)
		{
			_logger = logger;
			_integrationService = integrationService;
		}

		public async Task<bool> Handle(OrderToBillCommand command, CancellationToken cancellationToken)
		{
			var @event = new OrderReadyToBillMessage(command.OrderId);
			await _integrationService.PublishToEventBusAsync(@event, Program.AppName);

			return await Task.FromResult(true);
		}

	}
}
