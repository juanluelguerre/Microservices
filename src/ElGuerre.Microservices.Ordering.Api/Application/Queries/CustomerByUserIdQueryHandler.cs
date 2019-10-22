using ElGuerre.Microservices.Ordering.Api.Application.Extensions;
using ElGuerre.Microservices.Ordering.Api.Application.Models;
using ElGuerre.Microservices.Ordering.Api.Application.ViewModels;
using ElGuerre.Microservices.Ordering.Api.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Application.Queries
{
	public class CustomerByUserIdQueryHandler : IRequestHandler<CustomerByUserIdQuery, CustomerModel>
	{
		private readonly ILogger _logger;
		private readonly OrderingContext _dbContext;

		public CustomerByUserIdQueryHandler(ILogger<CustomerByUserIdQueryHandler> logger, OrderingContext dbContext)
		{
			_logger = logger;
			_dbContext = dbContext;
		}

		public async Task<CustomerModel> Handle(CustomerByUserIdQuery request, CancellationToken cancellationToken)
		{
			var customer = await _dbContext.Customers.FindAsync(request.UserId);
			return customer.ToCustomerModel();
		}
	}
}
