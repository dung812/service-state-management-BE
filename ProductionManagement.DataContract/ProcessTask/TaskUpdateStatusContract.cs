using ProductionManagement.DataContract.Base;
using ProductionManagement.DataContract.Enum;

namespace ProductionManagement.DataContract.Process
{
	public class TaskUpdateStatusContract : UpdateContract<Models.ProcessTask>
	{
		public ProcessTaskStatus Status { get; set; } = ProcessTaskStatus.Completed;
	}
}
