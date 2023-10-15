using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using ProductionManagement.DataContract.Audit;
using ProductionManagement.DataContract.Common;
using ProductionManagement.DataContract.Mapper;
using ProductionManagement.RepositoryLayer.Interfaces;
using ProductionManagement.ServiceLayer.Extensions;
using ProductionManagement.ServiceLayer.Interfaces;
using ProductionManagement.Models;

namespace ProductionManagement.ServiceLayer.Services
{
    public class AuditService : IAuditService
    {
        private readonly IAuditRepository _auditRepository;
        public AuditService(IAuditRepository auditRepository)
        {
            this._auditRepository = auditRepository;
        }

        public PagedList<Audit> GetAuditsAsync(AuditQueryCriteria filter)
        {
            var auditResult = _auditRepository.Entities;
            if (filter.UserName != null)
            {
                auditResult = FilterByUserName(auditResult, filter.UserName);
            }
            if (filter.Type != null)
            {
                auditResult = FilterByType(auditResult, filter.Type);
            }
            if (filter.TableName != null)
            {
                auditResult = FilterByTableName(auditResult, filter.TableName);
            }

            var result = Paginate(auditResult, filter.Limit, filter.Page);

            return result.ToPagedList<Audit>();
        }
        public PagedList<Audit> Paginate(IQueryable<Audit> audits, int limit, int page)
        {
            return audits.AsNoTracking().Paginate<Audit>(page, limit);

        }
        public IQueryable<Audit> FilterByUserName(IQueryable<Audit> audits, string userName)
        {
            return audits.Where(audit => audit.UserName == userName);
        }
        public IQueryable<Audit> FilterByType(IQueryable<Audit> audits, string userName)
        {
            return audits.Where(audit => audit.UserName == userName);
        }
        public IQueryable<Audit> FilterByTableName(IQueryable<Audit> audits, string userName)
        {
            return audits.Where(audit => audit.UserName == userName);
        }
    }
}
