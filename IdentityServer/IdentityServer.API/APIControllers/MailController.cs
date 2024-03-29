﻿using MenuOrder.Shared.Controller;
using MenuOrder.Shared.Models;
using MenuOrder.Shared.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer.API.APIControllers
{
    [Authorize(LocalApi.PolicyName)]
    public class MailController : BaseController
    {
        private readonly IMailService _mailService;
        private readonly IConfiguration _configuration;
        public MailController(IMailService mailService, IConfiguration configuration)
        {
            _mailService = mailService;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("/api/mail/send-mail")]
        public IActionResult SendEmailToRecipient([FromBody] MailBody mailBody)
        {
            var body = _mailService.SendEmailTemplateBody(mailBody.TemplateType,"","","");
            var result = _mailService.SendMailWithSingleRecipient(mailBody.ToAddress,mailBody.Subject, mailBody.Body);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("/api/mail/send-welcome-vendor")]
        public IActionResult SendWelcomeMailToVendor([FromBody] WelcomeVendorModel welcomeVendorModel)
        {
            var vendorRegisterUrl = _configuration.GetSection("MailSettings:VendorRegisterUrl").Get<string>();
            var body = _mailService.SendEmailTemplateBody(welcomeVendorModel.TemplateType, welcomeVendorModel.VendorName,
                welcomeVendorModel.Username, vendorRegisterUrl);

            var result = _mailService.SendMailWithSingleRecipient(welcomeVendorModel.VendorEmail, welcomeVendorModel.Subject, body);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("/api/mail/send-register-vendor")]
        public IActionResult SendRegisterVendorUser([FromBody] RegisterVendorUser registerVendorUser)
        {
            StringBuilder sendRegisterUrl = new StringBuilder(_configuration.GetSection("MailSettings:SendRegisterVendorUser").Get<string>());

            var body = _mailService.SendEmailTemplateBody(registerVendorUser.TemplateType, registerVendorUser.Vendorname,
                registerVendorUser.ToAddress, sendRegisterUrl.ToString(),registerVendorUser.VendorId);

            var result = _mailService.SendMailWithSingleRecipient(registerVendorUser.ToAddress, registerVendorUser.Subject, body);
            return Ok(result);
        }
    }
}
