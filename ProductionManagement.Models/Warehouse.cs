using ProductionManagement.Models.Base;
using System;

namespace ProductionManagement.Models
{
	public class Warehouse : Entity<Guid>
	{
		public string Name { get; set; } = null!;
		public string PhoneNumber { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Address { get; set; } = null!;
		public string MapImage { get; set; } = string.Empty;
	}
}
