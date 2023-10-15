using ProductionManagement.Models.Base;
using System;

namespace ProductionManagement.Models
{
	public class Workshop : Entity<Guid>
	{
		public string Name { get; set; } = null!;
		public string PhoneNumber { get; set; } = string.Empty;
		public string Address { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
	}
}
