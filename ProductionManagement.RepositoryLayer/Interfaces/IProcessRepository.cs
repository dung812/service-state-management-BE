using ProductionManagement.Models;
using System;

namespace ProductionManagement.RepositoryLayer.Interfaces
{
	public interface IProcessRepository : IBaseRepository<Process, Guid>
	{
	}
}
