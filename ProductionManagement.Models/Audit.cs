using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductionManagement.Models
{
    [Table("Audit")]
    public class Audit
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Type { get; set; } = null!;
        public string TableName { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public string OldValues { get; set; } = string.Empty;
		public string NewValues { get; set; } = string.Empty;
		public string AffectedColumns { get; set; } = string.Empty;
		public string PrimaryKey { get; set; } = string.Empty;
	}
}
