using ProductionManagement.Models.Base;
using System;
using System.Collections.Generic;

namespace ProductionManagement.Models
{
	public class Category : Entity<Guid>
	{
		public string Name { get; set; } = null!;
		public string Description { get; set; } = string.Empty;

		public Guid? ParentCategoryId { get; set; }
		public virtual Category? ParentCategory { get; set; }

		public virtual ICollection<Category>? ChildCategories { get; set; }
		public virtual ICollection<Material>? Materials { get; set; }
	}
}
