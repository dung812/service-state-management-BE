namespace ProductionManagement.DataContract.Common
{
	public class CreatedResponse
	{
		public string Id { get; set; } = null!;
		public CreatedResponse(string id) {
			Id = id;
		}
	}
}
