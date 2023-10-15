using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionManagement.DataContract.Role;
using ProductionManagement.Exceptions;
using ProductionManagement.ServiceLayer.Interfaces;

namespace ProductionManagement.API.Controllers.V1
{
	[ApiController, ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	[Authorize(Policy = "Admin")]
	public class RolesController : ControllerBase
	{
		private readonly IRoleService _roleService;

		public RolesController(IRoleService roleService)
		{
			_roleService = roleService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<RoleViewContract>>> GetRolesAsync()
		{
			return Ok(await _roleService.GetAllAsync());
		}

		[HttpPost]
		public async Task<ActionResult<RoleViewContract>> CreateRoleAsync([FromBody] RoleContract role)
		{
			RoleViewContract roleAdded = await _roleService.CreateAsync(role);
			return StatusCode(StatusCodes.Status201Created, roleAdded);
		}

		[HttpPut("{name}")]
		public async Task<ActionResult<RoleViewContract>> UpdateRoleAsync([FromRoute(Name = "name")] string oldName, [FromBody] RoleContract roleContract)
		{
			var roleToUpdate = await _roleService.GetByNameAsync(oldName);
			if (roleToUpdate.Name != oldName)
			{
				throw new ArgumentException("There are some errors when trying to get the requested role");
			}
			await _roleService.UpdateAsync(roleToUpdate, roleContract.Name);
			return NoContent();
		}

		[HttpDelete("{name}")]
		public async Task<ActionResult> DeleteRoleAsync([FromRoute(Name = "name")] string roleName)
		{
			var roleToDelete = await _roleService.GetByNameAsync(roleName);
			await _roleService.DeleteAsync(roleToDelete);
			return NoContent();
		}
	}
}
