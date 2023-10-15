using ProductionManagement.Models;
using System;

namespace ProductionManagement.RepositoryLayer.Interfaces
{
	public interface IProcessTaskRepository : IBaseRepository<ProcessTask, Guid>
	{
	}
}
