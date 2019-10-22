using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Customers;
using ElGuerre.Microservices.Ordering.Api.Domain.Aggregates.Orders;
using ElGuerre.Microservices.Ordering.Api.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Application.Queries
{
	public class QueryQuery : IOrderQuery
	{
		private readonly OrderingContext _dbContext;
		
		public QueryQuery(OrderingContext dbcontext)
		{
			_dbContext = dbcontext;
		}	

		public async Task<Order> FindByIdAsync(int orderId)
		{
			var order = await _dbContext.Orders.FindAsync(orderId);
			if (order != null)
			{
				await _dbContext.Entry(order)
					.Collection(a => a.OrderItems).LoadAsync();
				await _dbContext.Entry(order)
					.Reference(i => i.OrderStatus).LoadAsync();
				await _dbContext.Entry(order).Reference(a => a.Address)
					.LoadAsync();
			}
			return order;
		}
	}
}
