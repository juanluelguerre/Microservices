using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Application.Models
{
	public class PagedItem<T> where T : class
	{
		public PagedItem()
		{
			PageIndex = 1;
			PageSize = 10;
			Total = 0;
			Items = new List<T>();
		}

		public int PageSize { get; set; }
		public int PageIndex { get; set; }
		public long Total { get; set; }
		public IList<T> Items{ get; set; }
	}
}
