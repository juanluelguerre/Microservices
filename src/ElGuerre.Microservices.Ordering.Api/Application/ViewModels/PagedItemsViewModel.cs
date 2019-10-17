using System.Collections.Generic;

namespace ElGuerre.Microservices.Ordering.Api.Application.ViewModels
{
	public class PagedItemsViewModel<TEntity> where TEntity : class
	{
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
		public long Count { get; set; }
		/// <summary>
		/// List of paged items according to <see cref="PageSize"/> and <see cref="PageIndex"/>.
		/// Default value is an empty List of <typeparamref name="T"/>
		/// </summary>		
		public IEnumerable<TEntity> Data { get; private set; }

		public PagedItemsViewModel(int pageIndex, int pageSize, long count, IEnumerable<TEntity> data)
		{
			PageIndex = pageIndex;
			PageSize = pageSize;
			Count = count;
			Data = data;
		}
	}
}
