using ElGuerre.Microservices.Ordering.Api.Application.DomainEventHandlers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ElGuerre.Microservices.Sales.Api.Tests.Domain.Events
{
	public class OrderEventHandlersTests
	{
		private readonly IMediator _mediator;

		public OrderEventHandlersTests()
		{
			var services = new ServiceCollection();
			services.AddMediatR(typeof(OrderStartedEventHandler));
			_mediator = services.BuildServiceProvider().GetService<IMediator>();
		}

		[Fact]
		public Task GetOrdersOkTest()
		{
			throw new NotImplementedException();
		}
	}
}
