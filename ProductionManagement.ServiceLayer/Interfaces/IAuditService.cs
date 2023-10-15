using ProductionManagement.DataContract.Audit;
using ProductionManagement.DataContract.Common;
using ProductionManagement.Models;

namespace ProductionManagement.ServiceLayer.Interfaces
{
    public interface IAuditService
    {
        PagedList<Audit> GetAuditsAsync(AuditQueryCriteria filter);
    }
}
