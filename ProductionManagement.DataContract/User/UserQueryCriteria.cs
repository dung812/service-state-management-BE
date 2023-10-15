using ProductionManagement.DataContract.Common;
using ProductionManagement.DataContract.Enum;

namespace ProductionManagement.DataContract.User
{
	public class UserQueryCriteria : BaseQueryCriteria
	{
		public string? Role { get; set; }
	}
}
