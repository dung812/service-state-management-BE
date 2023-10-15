using Microsoft.AspNetCore.Identity;
using ProductionManagement.Models.Interfaces;
using System;

namespace ProductionManagement.Models
{
	public class User : IdentityUser, IEntity<string>
	{
		public string Name { get; set; } = null!;
		public bool IsDisabled { get; set; } = false;

		public DateTime CreatedDate { get; set; }
		public string? CreatedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string? ModifiedBy { get; set; }
		public DateTime? DeletedDate { get; set; }
		public string? DeletedBy { get; set; }
	}
}
