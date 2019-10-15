using ElGuerre.Microservices.Sales.Api.Application.Models;
using ElGuerre.Microservices.Sales.Api.Domain;
using ElGuerre.Microservices.Sales.Api.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Infrastructure.Repositories
{
	public class OrdersRepository : IOrdersRepository
	{
		private readonly OrdersContext _dbContext;

		public OrdersRepository(OrdersContext dbcontext)
		{
			_dbContext = dbcontext;
		}

		public async Task<Domain.Order> GetOrderById(int orderId)
		{
			return await _dbContext.Elementos.FindAsync(orderId);						
		}

		public async Task<IQueryable<Domain.Order>> GetOrders(int pageIndex, int pageSize)
		{
			var orders = _dbContext.Orders.AsQueryable();
			return orders;
		}

		public async Task<bool> Add(Domain.Order order)
		{
			_dbContext.Add(order);
			return await _dbContext.SaveChangesAsync() > 0;
		}

		public async Task<bool> Delete(int orderId)
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
