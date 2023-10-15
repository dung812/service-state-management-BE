using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProductionManagement.DataContract.Constant;
using ProductionManagement.DataContract.Mapper;
using ProductionManagement.DataContract.Process;
using ProductionManagement.Exceptions;
using ProductionManagement.Models;
using ProductionManagement.RepositoryLayer.Interfaces;
using ProductionManagement.ServiceLayer.Extensions;
using ProductionManagement.ServiceLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductionManagement.ServiceLayer.Services
{
	public class ProcessService : IProcessService
	{
		private readonly IProcessRepository _processRepository;
		private readonly IProcessTaskService _taskService;
		private readonly HttpContext _httpContext;
		public ProcessService(IProcessRepository processRepository, IProcessTaskService taskService, IHttpContextAccessor contextAccessor)
		{
			_processRepository = processRepository;
			_taskService = taskService;
			_httpContext = contextAccessor.HttpContext;
		}

		public async Task<ProcessSimpleViewContract> CreateProcessWithMultiTaskAsync(ProcessCreateContract createContract)
		{
			var processAdded = await _processRepository.AddAsync(createContract.ToNewEntity());
			foreach (var task in processAdded.Tasks!)
			{
				await _taskService.AssignToRolesAsync(task.Id.ToString(), task.RoleNames.Split(Separator.CommaSeparator));
			}
			return Mapping.Mapper.Map<ProcessSimpleViewContract>(processAdded);
		}

		private string[] GetAuthenticatedRoles()
		{
			return _httpContext.User.GetAuthenticatedRoles().Split(Separator.CommaSeparator, StringSplitOptions.RemoveEmptyEntries);
		}

		public async Task<ProcessDetailViewContract> GetAsDetailViewIncludedTask(string processId)
		{
			var processToResult = await GetByIdAsync(processId, nameof(Process.Tasks));
			var taskIdsAssignedTo = await _taskService.GetAllTaskIdsAssignedToRolesAsync(GetAuthenticatedRoles());

			var processDetailView = new ProcessDetailViewContract(processToResult, taskIdsAssignedTo);
			return processDetailView;
		}

		public async Task<Process> GetByIdAsync(string processId, params string[] includeProperties)
		{
			var query = _processRepository.Entities;
			foreach (var property in includeProperties)
			{
				query = query.Include(property);
			}
			return await query.FirstOrDefaultAsync(process => process.Id == Guid.Parse(processId))
				?? throw new NotFoundException($"Process with the id - {processId} doesn't exist");
		}

		public async Task<IEnumerable<Process>> GetListAsync(params string[] includeProperties)
		{
			var query = _processRepository.Entities;
			foreach (var property in includeProperties)
			{
				query = query.Include(property);
			}
			return await query.ToListAsync();
		}

		public async Task<IEnumerable<ProcessSimpleViewContract>> GetListViewAsync()
		{
			var processList = await GetListAsync(nameof(Process.Tasks));
			return Mapping.Mapper.Map<IEnumerable<ProcessSimpleViewContract>>(processList);
		}

		public void UpdateProcessInfo(Process processToUpdate, ProcessUpdateContract updateContract)
		{
			_processRepository.Update(updateContract.UpdateFor(processToUpdate));
		}
	}
}
