using System;

namespace ProductionManagement.Models.Interfaces
{
    public interface IEntity<TType>
	{
        TType? Id { get; set; }
		DateTime CreatedDate { get; set; }
		string? CreatedBy { get; set; }
		DateTime? ModifiedDate { get; set; }
		string? ModifiedBy { get; set; }
		public DateTime? DeletedDate { get; set; }
		public string? DeletedBy { get; set; }
	}
}
