using System.Collections.Generic;

namespace ProductionManagement.DataContract.Process
{
	public class GroupTaskViewContract
	{
		public int Level { get; set; }
		public List<TaskSimpleViewContract> Tasks { get; set; } = new ();
	}
}
