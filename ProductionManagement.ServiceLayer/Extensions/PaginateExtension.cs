using System.Linq;
using ProductionManagement.DataContract.Common;

namespace ProductionManagement.ServiceLayer.Extensions
{
	public static class PaginateExtension
	{
		public static PagedList<TModel> Paginate<TModel>(this IQueryable<TModel> entities, int pageNumber, int pageSize)
		{
			var paged = new PagedList<TModel>();
			pageNumber = (pageNumber < 1) ? 1 : pageNumber;
			pageSize = (pageSize < 1) ? 5 : pageSize;

			paged.CurrentPage = pageNumber;
			paged.PageSize = pageSize;

			var startRow = (pageNumber - 1) * pageSize;

			paged.Items = entities.Skip(startRow).Take(pageSize).ToList();

			paged.TotalItems = entities.Count();

			paged.TotalPages = ((paged.TotalItems - 1) / pageSize) + 1;

			return paged;
		}
	}
}
