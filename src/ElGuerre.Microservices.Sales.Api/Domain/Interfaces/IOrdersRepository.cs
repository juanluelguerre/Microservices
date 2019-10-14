using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Domain.Interfaces
{
	public interface IOrdersRepository
	{
		Task<Order> GetOrderById(int orderId);
		Task<IQueryable<Order>> GetOrders(int pageIndex, int pageSize);
		Task<bool> Add(Order order);
		Task<bool> Delete(int orderId);
		Task<bool> UpdateName(int orderId, string name);		
	}
}