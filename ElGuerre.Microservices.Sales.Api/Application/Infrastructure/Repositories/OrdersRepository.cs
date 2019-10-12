using ElGuerre.Microservices.Sales.Api.Domain;
using ElGuerre.Microservices.Sales.Api.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Application.Infrastructure.Repositories
{
	public class OrdersRepository : IOrdersRepository
	{
		private readonly OrdersContext _dbContext;

		public OrdersRepository(OrdersContext dbcontext)
		{
			_dbContext = dbcontext;
		}

		public bool Add(OrderEntity order)
		{
			throw new NotImplementedException();
		}

		public bool Delete(int orderId)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> UpdateName(int orderId, string name)
		{
			var order = await _dbContext.Orders.FindAsync(orderId);
			if (order != null)
			{
				order.Name = name;
				_dbContext.Update(order);
				return await _dbContext.SaveChangesAsync() > 0;
			}
			return false;
		}
	}
}
