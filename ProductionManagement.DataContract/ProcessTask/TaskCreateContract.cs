using ProductionManagement.DataContract.Base;
using ProductionManagement.DataContract.ProcessTask;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductionManagement.DataContract.Process
{
	public class TaskCreateContract : CreateContract<Models.ProcessTask>
	{
		[Required(ErrorMessage = "Role names are required", AllowEmptyStrings = false)]
		public string RoleNames { get; set; } = null!;
		[MaxLength(128, ErrorMessage = "Name must be less than 128 characters long")]
		[Required(ErrorMessage = "Name is required", AllowEmptyStrings = false)]
		public string Name { get; set; } = null!;
		public string? ProcessId { get; set; }
		public int Level { get; set; }
	}
}
