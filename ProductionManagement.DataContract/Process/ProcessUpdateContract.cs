using ProductionManagement.DataContract.Base;
using System.ComponentModel.DataAnnotations;

namespace ProductionManagement.DataContract.Process
{
	public class ProcessUpdateContract : UpdateContract<Models.Process>
	{
		[MaxLength(128, ErrorMessage = "Name must be less than 128 characters long")]
		[Required(ErrorMessage = "Name is required", AllowEmptyStrings = false)]
		public string Name { get; set; } = null!;
	}
}
