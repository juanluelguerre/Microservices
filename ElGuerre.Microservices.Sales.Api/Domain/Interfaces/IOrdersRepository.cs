using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Domain.Interfaces
{
	public interface IOrdersRepository
	{
		bool Add(OrderEntity order);
		bool Delete(int orderId);
		Task<bool> UpdateName(int orderId, string name);		
	}
}