using ElGuerre.Microservices.Sales.Api.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Services
{
    public interface IOrdersService	
    {
        Task<Order> GetItem(int id);
		Task<PagedItem<Order>> GetItems(int pageIndex, int pageSize);
		Task<bool> UpdateItem(Order order, bool sampleIsOk);
	}
}
