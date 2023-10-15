using ProductionManagement.Models;
using System;

namespace ProductionManagement.RepositoryLayer.Interfaces
{
	public interface IAuditRepository : IBaseRepository<Audit, Guid>
	{
	}
}
