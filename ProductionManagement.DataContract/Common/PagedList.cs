using ProductionManagement.DataContract.Mapper;
using System.Collections.Generic;

namespace ProductionManagement.DataContract.Common
{
	public class PagedList<TModel>
	{
		private const int MaxPageSize = 100;
		private int _pageSize;
		public int PageSize
		{
			get => _pageSize;
			set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
		}

		public int CurrentPage { get; set; }
		public int TotalItems { get; set; }
		public int TotalPages { get; set; }
		public IList<TModel> Items { get; set; }

		public PagedList<T> ToPagedList<T>()
		{

			return new PagedList<T>()
			{
				Items = Mapping.Mapper.Map<List<T>>(Items),
				CurrentPage = CurrentPage,
				TotalItems = TotalItems,
				TotalPages = TotalPages,
				PageSize = _pageSize
			};
		}

		public PagedList()
		{
			Items = new List<TModel>();
		}
	}
}
