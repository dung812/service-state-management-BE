using ProductionManagement.Models.Base;
using System;
using System.Collections.Generic;

namespace ProductionManagement.Models
{
	public class Unit : Entity<Guid>
	{
		public string Name { get; set; } = null!;
		public string Description { get; set; } = string.Empty;

		public virtual ICollection<Material>? Materials { get; set; }
		public virtual ICollection<ConvertDetail>? ConvertDetails { get; set; }
	}
}
