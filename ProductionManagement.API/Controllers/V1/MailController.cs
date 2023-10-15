using Microsoft.AspNetCore.Mvc;
using ProductionManagement.ServiceLayer.Constants;
using ProductionManagement.ServiceLayer.Interfaces;
using ProductionManagement.ServiceLayer.MailConfig;

namespace ProductionManagement.API.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMailSenderService _mail;

        public MailController(IMailSenderService mail)
        {
            _mail = mail;
        }

        [HttpPost("sendmail")]
        public async Task<IActionResult> SendMailAsync(Message message)
        {
            var result = await _mail.SendAsync(message);

            return result ? StatusCode(StatusCodes.Status200OK, EmailMessages.SUCCESS) : StatusCode(StatusCodes.Status500InternalServerError, EmailMessages.FAIL);
        }
    }
}
