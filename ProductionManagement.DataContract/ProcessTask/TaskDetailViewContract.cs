using System.Collections.Generic;

namespace ProductionManagement.DataContract.ProcessTask
{
	public class TaskDetailViewContract
	{
		public string Id { get; set; } = null!;
		public string Name { get; set; } = null!;
		public int Status { get; set; }
		public int Level { get; set; }
	}
}
