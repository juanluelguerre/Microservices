using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Customers
{
	public interface ICustomerQuery
	{
		Task<Customer> FindAsync(string customerIdentity);
		Task<Customer> FindByIdAsync(int customerId);
	}
}
