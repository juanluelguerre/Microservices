using ElGuerre.Microservices.Ordering.Api.Domain;
using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Customers;
using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders;
using ElGuerre.Microservices.Ordering.Api.Domain.Customers;
using ElGuerre.Microservices.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Infrastructure.Repositories
{
	public class CustomerRepository : ICustomerRepository
	{
		private readonly OrderingContext _dbContext;

		public IUnitOfWork UnitOfWork => _dbContext;

		public CustomerRepository(OrderingContext dbcontext)
		{
			_dbContext = dbcontext;
		}

		public Customer Add(Customer customer)
		{
			return _dbContext.Add(customer).Entity;			
		}

		public Customer Update(Customer customer)
		{
			_dbContext.Entry(customer).State = EntityState.Modified;
			return _dbContext.Entry(customer).Entity;
		}

	}
}
