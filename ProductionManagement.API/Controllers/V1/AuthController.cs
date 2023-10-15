using Microsoft.AspNetCore.Mvc;
using System.Net;
using ProductionManagement.DataContract.Identity;
using ProductionManagement.ServiceLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ProductionManagement.API.Controllers.V1
{
	[ApiController, ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	[AllowAnonymous]
	public class AuthController : ControllerBase
	{
		private readonly IIdentityService _service;

		public AuthController(IIdentityService service)
		{
			_service = service ?? throw new ArgumentException(nameof(service));
		}

		private static class RouteNames
		{
			public const string Login = nameof(Login);
		}

		[HttpPost("[action]", Name = RouteNames.Login)]
		[ProducesResponseType(typeof(LoginResponseContract), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> LoginAsync([FromBody] LoginContract loginModel)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var result = await _service.AuthorizeAsync(loginModel);
			return Ok(result);
		}
	}
}
