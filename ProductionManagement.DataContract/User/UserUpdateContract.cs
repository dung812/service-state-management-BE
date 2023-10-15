using ProductionManagement.DataContract.Base;
using System.ComponentModel.DataAnnotations;

namespace ProductionManagement.DataContract.User
{
	public class UserUpdateContract : UpdateContract<Models.User>
	{
		[MaxLength(255, ErrorMessage = "Name must be less than 255 characters long")]
		[Required(ErrorMessage = "Name is required", AllowEmptyStrings = false)]
		public string Name { get; set; } = null!;
	}
}
