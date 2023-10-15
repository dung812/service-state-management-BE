using Microsoft.AspNetCore.Identity;
using ProductionManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductionManagement.ServiceLayer.Interfaces
{
	public interface IProcessTaskService
	{
		Task<ProcessTask> GetByIdAsync(string processId, params string[] includeProperties);
		Task<bool> AssignToRoleAsync(string processId, IdentityRole role);
		Task AssignToRolesAsync(string processId, string[] roleNames);
		ProcessTask UpdateTaskInfo(ProcessTask process);
		Task<bool> RemoveFromRoleAsync(string processId, IdentityRole role);
		Task RemoveFromRolesAsync(string processId, string[] roleNames);
		Task RemoveAndAssignToRolesAsync(string processId, string[] newRoleNames);
		Task<IEnumerable<string>> GetAllTaskIdsAssignedToRoleAsync(string roleName);
		Task<IEnumerable<string>> GetAllTaskIdsAssignedToRolesAsync(string[] roleNames);
		Task<bool> IsAssignedToRolesAsync(ProcessTask processToCheck, string[] roleNames);
		Task<bool> MarkAsCompleletedAsync(ProcessTask processToUpdate);
	}
}
