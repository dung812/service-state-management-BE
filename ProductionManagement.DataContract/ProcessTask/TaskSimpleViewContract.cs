using ProductionManagement.DataContract.Enum;

namespace ProductionManagement.DataContract.Process
{
	public class TaskSimpleViewContract
	{
		public string? Id { get; set; }
		public string Name { get; set; } = null!;
		public string ViewStatus { get; set; } = ProcessTaskStatus.Processing.ToString();
	}
}
