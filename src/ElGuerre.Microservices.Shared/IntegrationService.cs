using ElGuerre.Microservices.Messages;
using ElGuerre.Microservices.Messages.Orders;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Shared.Infrastructure
{
	public class IntegrationService : IIntegrationService
	{
		private readonly ILogger _logger;
		private readonly IBus _eventBus;

		public IntegrationService(ILogger<IntegrationService> logger, IBus eventBus)
		{
			_logger = logger;
			_eventBus = eventBus;
		}

		public async Task PublishToEventBusAsync<T>(T message, string appName) where T : IEvent
		{
			try
			{
				message.CorrelationId = message.CorrelationId == Guid.Empty ? Guid.NewGuid() : message.CorrelationId;
				await _eventBus.Publish(message);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"ERROR publishing integration event: {message.CorrelationId} from {appName}");
			}
		}

		public async Task<Q> SendToEventBusAsync<T, Q>(T command, string appName) where T : class, ICommand<Q>  where Q : class
		{
			try
			{
				command.CorrelationId = command.CorrelationId == Guid.Empty ? Guid.NewGuid() : command.CorrelationId;

				// var serviceAddress = new Uri("rabbitmq://localhost/check-order-status");
				// var client = _eventBus.CreateRequestClient<T>(serviceAddress);
				var client = _eventBus.CreateRequestClient<T>();

				var response = await client.GetResponse<Q>(command);

				return response.Message;

			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"ERROR sending integration event: {command.CorrelationId} from {appName}");
			}

			return null;
		}

	}
}
