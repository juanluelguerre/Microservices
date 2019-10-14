using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElGuerre.Microservices.Sales.Api.Application.Models
{
	public class PagedItems<T> where T : class
	{
		public PagedItems()
		{
			PageIndex = 1;
			PageSize = 10;
			TotalItems = 0;
			Items = new List<T>();
		}

		/// <summary>
		/// Size of each page.
		/// </summary>
		public int PageSize { get; set; }
		/// <summary>
		/// Number of page is going to be recovered.
		/// </summary>
		public int PageIndex { get; set; }
		/// <summary>
		/// Total number of items.
		/// </summary>
		public long TotalItems { get; set; }
		/// <summary>
		/// List of paged items according to <see cref="PageSize"/> and <see cref="PageIndex"/>.
		/// Default value is an empty List of <typeparamref name="T"/>
		/// </summary>		
		public IList<T> Items{ get; set; }
	}
}
