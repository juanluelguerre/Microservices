using ElGuerre.Microservices.Ordering.Api.Domain;
using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Infrastructure.Repositories
{
	public class OrdersRepository : IOrderRepository
	{
		private readonly OrderingContext _dbContext;

		public IUnitOfWork UnitOfWork => _dbContext;

		public OrdersRepository(OrderingContext dbcontext)
		{
			_dbContext = dbcontext;
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
	}
}
