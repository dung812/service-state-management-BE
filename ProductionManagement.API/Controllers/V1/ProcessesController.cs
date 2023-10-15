using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionManagement.DataContract.Common;
using ProductionManagement.DataContract.Process;
using ProductionManagement.Exceptions;
using ProductionManagement.ServiceLayer.Interfaces;

namespace ProductionManagement.API.Controllers.V1
{
	[ApiController, ApiVersion("1.0")]
	[Route("api/v{version:apiVersion}/[controller]")]
	[Authorize]
	public class ProcessesController : ControllerBase
	{
		private readonly IProcessService _processService;
		public ProcessesController(IProcessService processService)
		{
			_processService = processService;
		}

		[HttpGet, ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<ProcessSimpleViewContract>>> GetProcesesAsync()
		{
			return Ok(await _processService.GetListViewAsync());
		}

		[HttpGet("{id}"), ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<ProcessDetailViewContract>> GetProcessAsync([FromRoute] string id)
		{
			return Ok(await _processService.GetAsDetailViewIncludedTask(id));
		}

		[HttpPost, ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<ActionResult<CreatedResponse>> CreateProcessAsync([FromBody] ProcessCreateContract createContract)
		{
			var processAdded = await _processService.CreateProcessWithMultiTaskAsync(createContract);
			return StatusCode(StatusCodes.Status201Created, new CreatedResponse(processAdded.Id));
		}

		[HttpPut("{id}/update"), ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<ActionResult> UpdateProcessInfoAsync([FromRoute] string id, [FromBody] ProcessUpdateContract updateContract)
		{
			var processToUpdate = await _processService.GetByIdAsync(id);
			if(processToUpdate.Id.ToString() != id)
			{
				throw new ArgumentException("There are some errors when trying to get the requested process");
			}
			_processService.UpdateProcessInfo(processToUpdate, updateContract);
			return NoContent();
		}
	}
}
