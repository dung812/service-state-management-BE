using System.ComponentModel.DataAnnotations;

namespace ProductionManagement.DataContract.User
{
	public class ChangeRolesContract
	{
		[Required(ErrorMessage = "Roles are required", AllowEmptyStrings = false)]
		public string Roles { get; set; } = null!;
	}
}
