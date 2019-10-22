using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders;
using ElGuerre.Microservices.Ordering.Api.Infrastructure.Repositories;
using ElGuerre.Microservices.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders
{
	public interface IOrderRepository : IRepository<Order>
	{
		Order Add(Order order);
		void Update(Order order);		
	}
}