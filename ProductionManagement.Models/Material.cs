using ProductionManagement.Models.Base;
using System;
using System.Collections.Generic;

namespace ProductionManagement.Models
{
	public class Material : Entity<Guid>
	{
		public string NameByDefault { get; set; } = null!;
		public string NameByCustomer { get; set; } = string.Empty;
		public string NameByVender { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public float ImportPrice { get; set; }
		public float SellingPrice { get; set; }
		public float MinimunQuantity { get; set; }
		public string? PrimaryImage { get; set; }
		public string? OtherImages { get; set; }

		public Guid CategoryId { get; set; }
		public virtual Category Category { get; set; } = null!;

		public Guid UnitId { get; set; }
		public virtual Unit Unit { get; set; } = null!;

		public virtual ICollection<ConvertDetail>? ConvertDetails { get; set; }
	}
}
