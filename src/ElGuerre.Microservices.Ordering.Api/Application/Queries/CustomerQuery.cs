using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Customers;
using ElGuerre.Microservices.Ordering.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Application.Queries
{
	public class CustomerQuery : ICustomerQuery
	{
		private readonly OrderingContext _dbContext;

		public CustomerQuery(OrderingContext dbcontext)
		{
			_dbContext = dbcontext;
		}

		public async Task<Customer> FindAsync(string customerIdentity)
		{
			var customer = await _dbContext.Customers
				.Include(b => b.PaymentMethods)
				.Where(b => b.Identity == customerIdentity)
				.SingleOrDefaultAsync();

			return customer;
		}

		public async Task<Customer> FindByIdAsync(int customerId)
		{
			var customer = await _dbContext.Customers
				.Include(b => b.PaymentMethods)
				.Where(b => b.Id == customerId)
				.SingleOrDefaultAsync();

			return customer;
		}
	}
}
