using ElGuerre.Microservices.Ordering.Api.Application.Models;
using ElGuerre.Microservices.Ordering.Api.Application.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Ordering.Api.Application.Queries
{
	public class OrderPagedQuery : IRequest<PagedItemsViewModel<OrderModel>>
	{
		public int PageIndex { get; private set; }
		public int PageSize { get; private set; }

		public OrderPagedQuery(int pageIndex, int pageSize)
		{
			PageIndex = pageIndex;
			PageSize = pageSize;
		}
	}
}
