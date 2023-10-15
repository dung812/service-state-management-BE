using ProductionManagement.DataContract.Common;

namespace ProductionManagement.DataContract.Audit
{
    public class AuditQueryCriteria : BaseQueryCriteria
    {
        public string? UserName { get; set; }
        public string? Type { get; set; }
        public string? TableName { get; set; }
    }
}
