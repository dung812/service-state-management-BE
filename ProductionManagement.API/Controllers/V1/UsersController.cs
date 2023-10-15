using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionManagement.API.Extensions;
using ProductionManagement.DataContract.Common;
using ProductionManagement.DataContract.User;
using ProductionManagement.Models;
using ProductionManagement.ServiceLayer.Interfaces;

namespace ProductionManagement.API.Controllers.V1
{
	[ApiController, ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	[Authorize]
	public class UsersController : ControllerBase
	{
		private readonly IUserService _userService;
		public UsersController(IUserService userService)
		{
			_userService = userService;
		}


		[HttpGet("{id}"), ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<UserViewContract>> GetUserAsync([FromRoute] string id)
		{
			return Ok(await _userService.GetAsViewByIdAsync(id));
		}

		[HttpGet, ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<PagedList<UserViewContract>> GetUsersAsync([FromQuery] UserQueryCriteria filter)
		{
			return Ok(_userService.GetAllWithPaging(filter));
		}

		[HttpPost, ProducesResponseType(StatusCodes.Status201Created)]
		[Authorize(Policy = "Admin")]
		public async Task<ActionResult<CreatedResponse>> CreateUserAsync([FromBody] UserCreateContract userContract)
		{
			var userAdded = await _userService.CreateAsync(userContract.ToNewEntity(), userContract.Roles.SplitAndRemoveBlank(), userContract.NewPassword);
			return StatusCode(StatusCodes.Status201Created, new CreatedResponse(userAdded.Id));
		}

		[HttpPut("{id}"), ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<NoContentResult> PutUserAsync([FromBody] UserUpdateContract userContract, [FromRoute] string id)
		{
			var userToUpdate = userContract.UpdateFor(await _userService.GetByIdAsync(id));
			EnsureIsValidUser(userToUpdate, id);
			await _userService.UpdateAsync(userToUpdate);
			return NoContent();
		}

		[HttpPut("{id}/changeRoles"), ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<NoContentResult> UpdateUserRolesAsync([FromBody] ChangeRolesContract rolesContract, [FromRoute] string id)
		{
			var userToUpdate = await _userService.GetByIdAsync(id);
			EnsureIsValidUser(userToUpdate, id);
			await _userService.UpdateRoleAsync(userToUpdate, rolesContract.Roles.SplitAndRemoveBlank());
			return NoContent();
		}

		[HttpPut("changePassword"), ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<NoContentResult> ChangeAuthenticatedUserPasswordAsync([FromBody] ChangePasswordContract changePasswordModel)
		{
			var userToChangePassword = await _userService.GetByUserNameAsync(User.GetAuthenticatedUsername());
			await _userService.ChangePasswordAsync(userToChangePassword, changePasswordModel.NewPassword, changePasswordModel.CurrentPassword);
			return NoContent();
		}

		[HttpPut("{id}/changePassword"), ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<NoContentResult> ChangeOtherUserPasswordAsync([FromRoute] string id, [FromBody] ChangePasswordContract changePasswordModel)
		{
			var userToChangePassword = await _userService.GetByIdAsync(id);
			EnsureIsValidUser(userToChangePassword, id);
			await _userService.ChangePasswordAsync(userToChangePassword, changePasswordModel.NewPassword);
			return NoContent();
		}

		[HttpPut("{id}/disable"), ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<NoContentResult> DisableUserAsync([FromRoute] string id)
		{
			await _userService.DisableAsync(id);
			return NoContent();
		}

		private static void EnsureIsValidUser(User user, string id)
		{
			if(user.Id != id)
				throw new ArgumentException("There are some errors when trying to get the requested user");
		}
	}
}
