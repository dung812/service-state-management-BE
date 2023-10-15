using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductionManagement.DataContract.Enum;
using ProductionManagement.DataContract.Mapper;
using ProductionManagement.DataContract.Process;
using ProductionManagement.Exceptions;
using ProductionManagement.Models;
using ProductionManagement.RepositoryLayer.Interfaces;
using ProductionManagement.ServiceLayer.Constants;
using ProductionManagement.ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProductionManagement.ServiceLayer.Services
{
	public class ProcessTaskService : IProcessTaskService
	{
		private readonly IProcessTaskRepository _taskRepository;
		private readonly IRoleService _roleService;

		public ProcessTaskService(IProcessTaskRepository taskRepository, IRoleService roleService)
		{
			_taskRepository = taskRepository;
			_roleService = roleService;
		}

		public async Task<ProcessTask> GetByIdAsync(string processId, params string[] includeProperties)
		{
			var query = _taskRepository.Entities;
			foreach (var property in includeProperties)
			{
				query = query.Include(property);
			}
			return await query.FirstOrDefaultAsync(process => process.Id == Guid.Parse(processId)) ?? throw new NotFoundException($"Process with the id - {processId} doesn't exist");
		}

		public async Task AssignToRolesAsync(string processId, string[] roleNames)
		{
			foreach (string roleName in roleNames)
			{
				var roleToAssign = await _roleService.GetByNameAsync(roleName);
				await AssignToRoleAsync(processId, roleToAssign);
			}
		}

		public async Task<bool> AssignToRoleAsync(string processId, IdentityRole role)
		{
			return await _roleService.AddClaimAsync(role, new Claim(CustomClaimTypes.RoleTasks, processId));
		}

		public async Task<bool> RemoveFromRoleAsync(string processId, IdentityRole role)
		{
			return await _roleService.RemoveClaimAsync(role, new Claim(CustomClaimTypes.RoleTasks, processId));
		}

		public async Task<IEnumerable<string>> GetAllTaskIdsAssignedToRoleAsync(string roleName)
		{
			var claims = await _roleService.GetClaimsAsync(_roleService.GetByNameAsync(roleName).Result, CustomClaimTypes.RoleTasks);
			return claims.Select(claim => claim.Value);
		}

		public async Task<IEnumerable<string>> GetAllTaskIdsAssignedToRolesAsync(string[] roleNames)
		{
			var processIds = new List<string>();
			foreach (var roleName in roleNames)
			{
				processIds.AddRange(await GetAllTaskIdsAssignedToRoleAsync(roleName));
			}
			return processIds;
		}

		public async Task RemoveFromRolesAsync(string processId, string[] roleNames)
		{
			foreach (var roleName in roleNames)
			{
				var roleToAssign = await _roleService.GetByNameAsync(roleName);
				await RemoveFromRoleAsync(processId, roleToAssign);
			}
		}

		public async Task RemoveAndAssignToRolesAsync(string processId, string[] newRoleNames)
		{
			var oldRoleNames = await _roleService.GetRoleNamesForClaimAsync(new Claim(CustomClaimTypes.RoleTasks, processId));
			await RemoveFromRolesAsync(processId, oldRoleNames.ToArray());
			await AssignToRolesAsync(processId, newRoleNames);
		}

		public async Task<bool> MarkAsCompleletedAsync(ProcessTask taskToUpdate)
		{
			if (taskToUpdate.Status != (int)ProcessTaskStatus.Processing)
			{
				throw new ModifyException("The process is waiting or have compeleted");
			}

			taskToUpdate.Status = (int)ProcessTaskStatus.Completed;

			var notCompleteTasksInSameLevel = await _taskRepository.Entities.Where(task =>
				task.ProcessId == taskToUpdate.ProcessId &&
				task.Id != taskToUpdate.Id &&
				task.Level == taskToUpdate.Level &&
				task.Status != (int)ProcessTaskStatus.Completed
			).CountAsync();

			if (notCompleteTasksInSameLevel == 0)
			{
				var tasksInNextLevel = _taskRepository.Entities
					.Where(task => task.ProcessId == taskToUpdate.ProcessId && task.Level > taskToUpdate.Level)
					.AsEnumerable()
					.GroupBy(task => new { task.ProcessId, task.Level })
					.OrderBy(task => task.Key.Level)
					.FirstOrDefault();

				if (tasksInNextLevel != null)
				{
					taskToUpdate.Process!.CurrentLevel = tasksInNextLevel.Key.Level;
					tasksInNextLevel.ToList().ForEach(task =>
					{
						task.Status = (int)ProcessTaskStatus.Processing;
						_taskRepository.ChangeTracking(task, EntityState.Modified);
					});
				}
				else
				{
					taskToUpdate.Process!.CurrentLevel = taskToUpdate.Level;
				}

				_taskRepository.ChangeTracking(taskToUpdate, EntityState.Modified);
			}

			await _taskRepository.SaveChangesAsync(); //To make sure all update is in 1 transaction
			return taskToUpdate.Status == (int)ProcessTaskStatus.Completed;
		}

		public async Task<bool> IsAssignedToRolesAsync(ProcessTask processToCheck, string[] roleNames)
		{
			var assignedRoles = await _roleService.GetRoleNamesForClaimAsync(new Claim(CustomClaimTypes.RoleTasks, processToCheck.Id.ToString()));
			return assignedRoles.Any(assignedRole => roleNames.Contains(assignedRole, StringComparer.OrdinalIgnoreCase));
		}

		public ProcessTask UpdateTaskInfo(ProcessTask process)
		{
			return _taskRepository.Update(process);
		}
	}
}
