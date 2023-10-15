using System;

namespace ProductionManagement.DataContract.User
{
	public class UserViewContract
	{
		public string Id { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public string UserName { get; set; } = string.Empty;
		public string Roles { get; set; } = string.Empty;
		public bool IsDisabled { get; set; }
		public string Email { get; set; } = string.Empty;
		public DateTime? CreatedDate { get; set; }
		public DateTime? ModifiedDate { get; set; }
	}
}
