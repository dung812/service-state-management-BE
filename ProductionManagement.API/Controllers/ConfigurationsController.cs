using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProductionManagement.DataContract.Common;

namespace ProductionManagement.API.Controllers
{
	[Route("/")]
	[ApiController]
	[AllowAnonymous]
	public class ConfigurationsController : ControllerBase
	{
		private readonly IOptions<ApiConfigurations> _apiConfiguration;

		public ConfigurationsController(IOptions<ApiConfigurations> apiConfiguration)
		{
			_apiConfiguration = apiConfiguration;
		}

		[HttpGet]
		public ActionResult<ApiConfigurations> GetConfiguration()
		{
			return Ok(_apiConfiguration.Value);
		}
	}
}
