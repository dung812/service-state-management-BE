using ProductionManagement.Models.Interfaces;
using System;

namespace ProductionManagement.Models.Base
{
	public class Entity<TType> : IEntity<TType>
	{
		public TType? Id { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
		public string? CreatedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string? ModifiedBy { get; set; }
		public DateTime? DeletedDate { get; set; }
		public string? DeletedBy { get; set; }
	}
}
