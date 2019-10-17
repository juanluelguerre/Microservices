using ElGuerre.Microservices.Ordering.Api.Application.Models;
using ElGuerre.Microservices.Ordering.Api.Application.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ElGuerre.Microservices.Sales.Api.Tests.Application.Commands
{
	public class OrderCommandHandlerTests
	{
		private readonly IMediator _mediator;

		public OrderCommandHandlerTests()
		{
			var services = new ServiceCollection();

			// services.AddMediatR(typeof(OrderUpdateNameCommandHandler));
			services.AddMediatR(typeof(OrdersByIdQueryHandler));
			services.AddMediatR(typeof(OrdersPagedQueryHandler));

			_mediator = services.BuildServiceProvider().GetService<IMediator>();
		}

		[Fact]
		public async Task OrdersUpdateNameCommandHanderTest()
		{
			// var result = await _mediator.Send(new OrderUpdateNameCommand(1, "New Order 1"));

		}


		[Fact]
		public async Task OrdersByIdQueryHanderTest()
		{
			const int EXPECTED_ORDER = 1;
			var order = await _mediator.Send(new OrdersByIdQuery(EXPECTED_ORDER));
			Assert.Null(order);
			Assert.IsType<Order>(order);
			Assert.Equal(EXPECTED_ORDER, order.OrderId);
		}
	}
}
