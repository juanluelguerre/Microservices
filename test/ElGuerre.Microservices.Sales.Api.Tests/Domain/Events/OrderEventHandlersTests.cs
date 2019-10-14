using ElGuerre.Microservices.Sales.Api.Domain.Events;
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
			services.AddMediatR(typeof(OrderNameUpdatedHandler));
			_mediator = services.BuildServiceProvider().GetService<IMediator>();
		}

		[Fact]
		public Task GetOrdersOkTest()
		{
			throw new NotImplementedException();
		}
	}
}
