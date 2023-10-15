using ProductionManagement.DataContract.Process;
using ProductionManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductionManagement.ServiceLayer.Interfaces
{
	public interface IProcessService
	{
		Task<ProcessSimpleViewContract> CreateProcessWithMultiTaskAsync(ProcessCreateContract createContract);
		Task<IEnumerable<ProcessSimpleViewContract>> GetListViewAsync();
		Task<IEnumerable<Process>> GetListAsync(params string[] includeProperties);
		Task<Process> GetByIdAsync(string processId, params string[] includeProperties);
		Task<ProcessDetailViewContract> GetAsDetailViewIncludedTask(string processId);
		void UpdateProcessInfo(Process processToUpdate, ProcessUpdateContract updateContract);
	}
}
