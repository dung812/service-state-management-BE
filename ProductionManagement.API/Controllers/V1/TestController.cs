using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ProductionManagement.Models;

namespace ProductionManagement.API.Controllers.V1
{
	[ApiController]
	[Route("api/v{version:apiVersion}/[controller]")]
	public class TestController : ControllerBase
	{
		private readonly UserManager<User> _userManager;

		public TestController(UserManager<User> userManager)
		{
			_userManager = userManager;
		}

		[HttpPatch("update/{username}")]
		public async Task<IActionResult> Test([FromRoute] string username, [FromBody] JsonPatchDocument<User> patchDocument)
		{
			var userToUpdate = await _userManager.FindByNameAsync(username);
			patchDocument.ApplyTo(userToUpdate);
			return Ok(userToUpdate);


			//var processToUpdate = new Process { Name = username, Tasks = new };
		}
	}
}
