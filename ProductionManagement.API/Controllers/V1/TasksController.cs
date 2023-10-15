using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionManagement.API.Extensions;
using ProductionManagement.DataContract.Process;
using ProductionManagement.DataContract.ProcessTask;
using ProductionManagement.Exceptions;
using ProductionManagement.Models;
using ProductionManagement.ServiceLayer.Interfaces;

namespace ProductionManagement.API.Controllers.V1
{
	[ApiController, ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	[Authorize]
	public class TasksController : ControllerBase
	{
		private readonly IProcessTaskService _taskService;
		public TasksController(IProcessTaskService taskService)
		{
			_taskService = taskService;
		}

		[HttpPut("{id}/reassign"), ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<ActionResult> ReassignFromRoleAsync([FromRoute] string id, [FromBody] TaskReassignContract reassignContract)
		{
			await _taskService.RemoveAndAssignToRolesAsync(id, reassignContract.NewRoleNames.SplitAndRemoveBlank());
			return NoContent();
		}

		[HttpPut("{id}/update"), ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<ActionResult> UpdateTaskInfoAsync([FromRoute] string id, [FromBody] TaskUpdateInfoContract updateContract)
		{
			var taskToUpdate = await _taskService.GetByIdAsync(id);
			EnsureIsValidTask(taskToUpdate, id);
			_taskService.UpdateTaskInfo(updateContract.UpdateFor(taskToUpdate));
			return NoContent();
		}

		[HttpPut("{id}/complete"), ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<ActionResult> MarkProcessCompleltedAsync([FromRoute] string id)
		{
			var taskToUpdate = await _taskService.GetByIdAsync(id, nameof(ProcessTask.Process));
			EnsureIsValidTask(taskToUpdate, id);
			var isCanUpdate = await _taskService.IsAssignedToRolesAsync(taskToUpdate, User.GetAuthenticatedRoles().SplitAndRemoveBlank());
			if (!isCanUpdate)
			{
				throw new RestrictedPermissionException("You don't have permission to update this task");
			}
			await _taskService.MarkAsCompleletedAsync(taskToUpdate);
			return NoContent();
		}

		private static void EnsureIsValidTask(ProcessTask task, string id)
		{
			if (task.Id.ToString() != id)
				throw new ArgumentException("There are some errors when trying to get the requested task");
		}
	}
}
