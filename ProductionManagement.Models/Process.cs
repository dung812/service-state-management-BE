using ProductionManagement.Models.Base;
using System;
using System.Collections.Generic;

namespace ProductionManagement.Models
{
	public class Process : Entity<Guid>
	{
		public string Name { get; set; } = null!;
		public int CurrentLevel { get; set; } = 1;

		public virtual ICollection<ProcessTask>? Tasks { get; set; }
	}
}
