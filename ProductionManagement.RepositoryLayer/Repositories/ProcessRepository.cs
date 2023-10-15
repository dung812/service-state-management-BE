using ProductionManagement.DataAccessLayer.Context;
using ProductionManagement.Models;
using ProductionManagement.RepositoryLayer.Interfaces;
using System;

namespace ProductionManagement.RepositoryLayer.Repositories
{
	public class ProcessRepository : BaseRepository<Process, Guid>, IProcessRepository
	{
		public ProcessRepository(ProductionManagementContext dbContext) : base(dbContext)
		{
		}
	}
}
