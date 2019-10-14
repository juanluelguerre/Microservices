using ElGuerre.Microservices.Sales.Api.Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Application.Queries
{
	public class OrdersPagedQuery : IRequest<PagedItems<Order>>
	{
		public int PageIndex { get; private set; }
		public int PageSize { get; private set; }

		public OrdersPagedQuery(int pageIndex, int pageSize)
		{
			PageIndex = pageIndex;
			PageSize = pageSize;
		}
	}
}
