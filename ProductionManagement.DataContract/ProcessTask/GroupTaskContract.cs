using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductionManagement.DataContract.Process
{
	public class GroupTaskContract
	{
		[MinLength(1, ErrorMessage = "A step of process must have at least one task")]
		public List<TaskCreateContract> Tasks { get; set; } = new();

		public void SetLevelTasksInside(int level)
		{
			Tasks.ForEach(task => task.Level = level);
		}
	}
}
