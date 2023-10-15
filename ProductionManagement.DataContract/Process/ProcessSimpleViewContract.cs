namespace ProductionManagement.DataContract.Process
{
	public class ProcessSimpleViewContract
	{
		public string Id { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public int TotalTask { get; set; }
	}
}
