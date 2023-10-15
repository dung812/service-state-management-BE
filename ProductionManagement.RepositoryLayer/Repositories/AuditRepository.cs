using ProductionManagement.DataAccessLayer.Context;
using ProductionManagement.Models;
using ProductionManagement.RepositoryLayer.Interfaces;
using System;

namespace ProductionManagement.RepositoryLayer.Repositories
{
	public class AuditRepository : BaseRepository<Audit, Guid>, IAuditRepository
	{
		public AuditRepository(ProductionManagementContext dbContext) : base(dbContext)
		{
		}
	}
}
