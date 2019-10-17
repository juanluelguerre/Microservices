using ElGuerre.Microservices.Ordering.Api.Domain;
using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Customers;
using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders;
using ElGuerre.Microservices.Ordering.Api.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Infrastructure.Repositories
{
	public class CustomerRepository : ICustomerRepository
	{		 
        private readonly OrdersContext _context;
		public IUnitOfWork UnitOfWork
		{
			get
			{
				return _context;
			}
		}

		public CustomerRepository(OrdersContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public Customer Add(Customer customer)
		{
			if (customer.IsTransient())
			{
				return _context.Customers
					.Add(customer)
					.Entity;
			}
			else
			{
				return customer;
			}
		}

		public Customer Update(Customer customer)
		{
			return _context.Customers
					.Update(customer)
					.Entity;
		}

		public async Task<Customer> FindAsync(string customerIdentity)
		{
			var customer = await _context.Customers
				.Include(b => b.PaymentMethods)
				.Where(b => b.Identity == customerIdentity)
				.SingleOrDefaultAsync();

			return customer;
		}

		public async Task<Customer> FindByIdAsync(int customerId)
		{
			var customer = await _context.Customers
				.Include(b => b.PaymentMethods)
				.Where(b => b.Id == customerId)
				.SingleOrDefaultAsync();

			return customer;
		}
	}
}
