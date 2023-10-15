using ProductionManagement.DataContract.Enum;

namespace ProductionManagement.DataContract.Common
{
	public class BaseQueryCriteria
	{
		public string? SearchString { get; set; }
		public int Limit { get; set; } = 10;
		public int Page { get; set; } = 1;
		public ESortOrder? SortOrder { get; set; } = ESortOrder.Ascending;
		public string? SortColumn { get; set; } = string.Empty;
	}
}
