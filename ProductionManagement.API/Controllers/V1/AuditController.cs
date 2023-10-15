using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductionManagement.DataContract.Audit;
using ProductionManagement.Models;
using ProductionManagement.ServiceLayer.Interfaces;

namespace ProductionManagement.API.Controllers.V1
{
    [ApiController, ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Policy = "Admin")]

    public class AuditController : ControllerBase
    {
        private readonly IAuditService _auditService;

        public AuditController(IAuditService auditService)
        {
            _auditService = auditService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Audit>> GetAudits([FromQuery] AuditQueryCriteria filter)
        {
            return Ok(_auditService.GetAuditsAsync(filter));
        }
    }
}
