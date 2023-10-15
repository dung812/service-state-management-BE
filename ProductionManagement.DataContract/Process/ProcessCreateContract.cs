using ProductionManagement.DataContract.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ProductionManagement.DataContract.Process
{
	public class ProcessCreateContract : CreateContract<Models.Process>
	{
		[MaxLength(128, ErrorMessage = "Name must be less than 128 characters long")]
		[Required(ErrorMessage = "Name is required", AllowEmptyStrings = false)]
		public string Name { get; set; } = null!;

		private List<GroupTaskContract> _taskGroups = new();

		[MinLength(1, ErrorMessage = "Process must have at least one step")]
		public List<GroupTaskContract> TaskGroups
		{
			get
			{
				return _taskGroups;
			}
			set
			{
				_taskGroups = value;
				for (int i = 0; i < _taskGroups.Count; i++)
				{
					_taskGroups[i].SetLevelTasksInside(i + 1);
				}
			}
		}

		public IEnumerable<TaskCreateContract> FlatGroup()
		{
			return TaskGroups.SelectMany(taskGroup => taskGroup.Tasks);
		}
	}
}
