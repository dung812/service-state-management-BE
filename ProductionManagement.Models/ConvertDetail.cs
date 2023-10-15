using ProductionManagement.Models.Base;
using System;

namespace ProductionManagement.Models
{
	public class ConvertDetail
	{
		public Guid MaterialId { get; set; }
		public Guid UnitId { get; set; }

		public float Value { get; set; }

		public virtual Material? Material { get; set; }
		public virtual Unit? Unit { get; set; }
		public DateTime CreatedDate { get; set; }
		public string? CreatedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string? ModifiedBy { get; set; }
	}
}
