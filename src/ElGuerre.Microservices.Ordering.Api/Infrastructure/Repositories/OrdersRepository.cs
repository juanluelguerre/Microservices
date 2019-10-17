using ElGuerre.Microservices.Ordering.Api.Domain;
using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders;
using ElGuerre.Microservices.Ordering.Api.Domain.Orders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Infrastructure.Repositories
{
	public class OrdersRepository : IOrdersRepository
	{
		private readonly OrdersContext _dbContext;

		public IUnitOfWork UnitOfWork => _dbContext;

		public OrdersRepository(OrdersContext dbcontext)
		{
			_dbContext = dbcontext;
		}

		public async Task<Order> GetByIdAsync(int orderId)
		{
			var order = await _dbContext.Orders.FindAsync(orderId);
			if (order != null)
			{
				await _dbContext.Entry(order).Reference(a => a.OrderItems)
					.LoadAsync();
				await _dbContext.Entry(order)
					.Reference(i => i.OrderStatus).LoadAsync();
				await _dbContext.Entry(order).Reference(a => a.Address)
					.LoadAsync();
			}

			return order;
		}

		public async Task<IQueryable<Order>> GetAsync(int pageIndex, int pageSize)
		{
			var orders = _dbContext.Orders.AsQueryable();
			return orders;
		}

		public Order Add(Order order)
		{
			return _dbContext.Add(order).Entity;
			// return await _dbContext.SaveChangesAsync() > 0;
		}

		public void Update(Order order)
		{
			_dbContext.Entry(order).State = EntityState.Modified;
		}

		//public async Task<bool> UpdateName(int orderId, string name)
		//{
		//	var order = await _dbContext.Orders.FindAsync(orderId);
		//	if (order != null)
		//	{
		//		order.UpdateName(name);

		//		_dbContext.Update(order);
		//		return await _dbContext.SaveChangesAsync() > 0;
		//	}
		//	return false;
		//}
	}
}
