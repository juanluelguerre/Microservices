using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Customers;
using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders;
using ElGuerre.Microservices.Ordering.Api.Infrastructure.Repositories;
using ElGuerre.Microservices.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Domain.Customers
{
	public interface ICustomerRepository : IRepository<Customer>
	{
		Customer Add(Customer buyer);
		Customer Update(Customer buyer);
	}
}