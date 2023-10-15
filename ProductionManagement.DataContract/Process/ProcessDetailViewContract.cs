using ProductionManagement.DataContract.Base;
using ProductionManagement.DataContract.Enum;
using ProductionManagement.DataContract.Mapper;
using System.Collections.Generic;
using System.Linq;

namespace ProductionManagement.DataContract.Process
{
	public class ProcessDetailViewContract : CreateContract<Models.Process>
	{
		public string Id { get; set; } = null!;
		public string Name { get; set; } = null!;
		public int StepCount { get; set; }
		public int CurrentStep { get; set; }
		public IEnumerable<GroupTaskViewContract>? TaskGroups { get; set; }

		public ProcessDetailViewContract() { }

		public ProcessDetailViewContract(Models.Process process, IEnumerable<string> allowedTaskIds)
		{
			Id = process.Id.ToString();
			Name = process.Name;
			CurrentStep = process.CurrentLevel;
			TaskGroups = process.Tasks.Where(task => task.Level <= process.CurrentLevel)
				.GroupBy(task => task.Level)
				.OrderBy(group => group.Key)
				.Select(group =>
				{
					var assignedTasks = group.Where(task => allowedTaskIds.Any(taskId => taskId == task.Id.ToString()));
					var isAllTaskCompleted = group.All(task => task.Status == (int)ProcessTaskStatus.Completed);
					return new GroupTaskViewContract
					{
						Level = group.Key,
						Tasks = Mapping.Mapper.Map<List<TaskSimpleViewContract>>(isAllTaskCompleted ? group.ToList() : assignedTasks)
					};
				});
			StepCount = process.Tasks.GroupBy(task => task.Level).Count();
		}
	}
}
