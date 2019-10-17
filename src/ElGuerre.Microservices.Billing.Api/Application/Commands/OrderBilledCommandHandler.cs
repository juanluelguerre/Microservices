//using ElGuerre.Microservices.Billing.Api.Domain.Events;
//using ElGuerre.Microservices.Billing.Api.Domain.Interfaces;
//using ElGuerre.Microservices.Billing.Api.Infrastructure.Repositories;
//using ElGuerre.Microservices.Messages;
//using ElGuerre.Microservices.Sales.Api.Application.IntegrationEvents;
//using MediatR;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//namespace ElGuerre.Microservices.Billing.Api.Application.Commands
//{
//	public class OrderBilledCommandHandler : AsyncRequestHandler<OrderBilledCommand>
//	{
//		private readonly ILogger _logger;
//		private readonly IMediator _mediator;
//		private readonly IPayRepository _repository;
//		private readonly IIntegrationService _integrationService;

//		public OrderBilledCommandHandler(ILogger<OrderBilledCommandHandler> logger, IMediator mediator, IPayRepository repository, IIntegrationService integrationService)
//		{
//			_logger = logger;
//			_mediator = mediator;
//			_repository = repository;
//			_integrationService = integrationService;
//		}

//		protected override async Task Handle(OrderBilledCommand request, CancellationToken cancellationToken)
//		{



//		}
//	}
//}
