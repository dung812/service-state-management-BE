using ProductionManagement.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductionManagement.Models
{
	public class ProcessTask : Entity<Guid>
	{
		public string Name { get; set; } = null!;
		public int Status { get; set; }
		public int Level { get; set; }

		public Guid? ProcessId { get; set; }
		public virtual Process? Process { get; set; }

		[NotMapped]
		public string RoleNames { get; set; } = null!;
	}
}
