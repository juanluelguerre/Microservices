//using ElGuerre.Microservices.Messages;
//using ElGuerre.Microservices.Ordering.Api.Infrastructure;
//using ElGuerre.Microservices.Ordering.Api.Application.Models;
//using ElGuerre.Microservices.Ordering.Api.Application.Queries;
//using ElGuerre.Microservices.Ordering.Api.Application.Sagas;
//using ElGuerre.Microservices.Ordering.Api.Services;
//using GreenPipes;
//using MassTransit;
//using MassTransit.Saga;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using ElGuerre.Microservices.Ordering.Api.Application.IntegrationEvents;
//using ElGuerre.Microservices.Shared.Infrastructure;
//using ElGuerre.Microservices.Ordering.Api.Domain.Orders;

//namespace ElGuerre.Microservices.Ordering.Api.Application.Services
//{
//	public class OrdersService : IOrdersService
//	{
//		private readonly ILogger _logger;
//		private readonly IMediator _mediator;
//		private readonly IOrdersRepository _repository;
//		private readonly IIntegrationService _integrationService;

//		public OrdersService(ILogger<OrdersService> logger, IMediator mediator, IOrdersRepository repository, IIntegrationService integrationService)
//		{
//			_logger = logger;
//			_mediator = mediator;
//			_repository = repository;
//			_integrationService = integrationService;
//		}

//		public async Task<Order> GetOrder(int orderId)
//		{
//			var query = new OrdersByIdQuery(orderId);
//			return await _mediator.Send(query);;
//		}

//		public async Task<PagedItems<Order>> GetOrders(int pageIndex, int pageSize)
//		{
//			var query = new OrdersPagedQuery(pageIndex, pageSize);
//			return await _mediator.Send(query);
//		}

//		public async Task UpdateToBilled(int orderId)
//		{
//			// TODO: Review
//			//var order = await _repository.GetByIdAsync(orderId);
//			//order.SetPaidStatus();
//			//_repository.UnitOfWork.SaveChangesAsync();
//		}

//		public async Task<bool> PublishToBill(int orderId)
//		{
//			// TODO: Review
//			// await _integrationService.PublishToEventBusAsync<OrderBilledMessage>(new OrderBilledMessage(orderId), Program.AppName);
//			// return true;
//			return await Task.FromResult(true);
//		}

//		public async Task<bool> UpdateOrderName(int orderId, string name)
//		{
//			return await Task.FromResult(true);
//		}
//	}
//}
