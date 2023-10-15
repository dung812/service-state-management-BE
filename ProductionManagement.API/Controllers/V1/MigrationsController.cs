using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionManagement.DataContract.Migration;
using ProductionManagement.ServiceLayer.Interfaces;

namespace ProductionManagement.API.Controllers.V1
{
	[ApiController, ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	[Authorize(Policy = "Admin")]
	public class MigrationsController : ControllerBase
	{
		private readonly IMigrationService _migrationService;
		public MigrationsController(IMigrationService migrationService)
		{
			_migrationService = migrationService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<MigrationViewContract>>> GetHistoriesAsync()
		{
			return Ok(await _migrationService.GetAllAppliedMigrationsAsync());
		}
	}
}
