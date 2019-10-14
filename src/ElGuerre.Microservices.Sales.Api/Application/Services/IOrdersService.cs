using ElGuerre.Microservices.Sales.Api.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Services
{
    public interface IOrdersService	
    {
        Task<Order> GetOrder(int orderId);
		Task<PagedItems<Order>> GetOrders(int pageIndex, int pageSize);
		Task<bool> UpdateOrderName(int orderId, string name);
	}
}
