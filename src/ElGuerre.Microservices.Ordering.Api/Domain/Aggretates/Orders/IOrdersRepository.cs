using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders;
using ElGuerre.Microservices.Ordering.Api.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Domain.Orders
{
	public interface IOrdersRepository : IRepository<Order>
	{
		Task<Order> GetByIdAsync(int orderId);
		Task<IQueryable<Order>> GetAsync(int pageIndex, int pageSize);
		Order Add(Order order);
		void Update(Order order);		
	}
}