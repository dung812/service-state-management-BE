using ProductionManagement.DataContract.Base;
using System.ComponentModel.DataAnnotations;

namespace ProductionManagement.DataContract.ProcessTask
{
	public class TaskUpdateInfoContract : UpdateContract<Models.ProcessTask>
	{
		[MaxLength(128, ErrorMessage = "Name must be less than 128 characters long")]
		[Required(ErrorMessage = "Name is required", AllowEmptyStrings = false)]
		public string Name { get; set; } = null!;
	}
}
