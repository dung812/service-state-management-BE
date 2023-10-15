using ProductionManagement.DataAccessLayer.Context;
using ProductionManagement.Models;
using ProductionManagement.RepositoryLayer.Interfaces;
using System;

namespace ProductionManagement.RepositoryLayer.Repositories
{
	public class ProcessTaskRepository : BaseRepository<ProcessTask, Guid>, IProcessTaskRepository
	{
		public ProcessTaskRepository(ProductionManagementContext dbContext) : base(dbContext){}
	}
}
