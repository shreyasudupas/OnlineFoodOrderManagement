using MenuOrder.Shared.Controller;
using MenuOrder.Shared.Models;
using MenuOrder.Shared.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer.API.APIControllers
{
    [Authorize(LocalApi.PolicyName)]
    public class MailController : BaseController
    {
        private readonly IMailService _mailService;
        public MailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [AllowAnonymous]
        [HttpPost("/api/mail/send-mail")]
        public IActionResult SendEmailToRecipient([FromBody] MailBody mailBody)
        {
            var result = _mailService.SendMailWithSingleRecipient(mailBody.ToAddress,mailBody.Subject, mailBody.Body);
            return Ok(result);
        }
    }
}
