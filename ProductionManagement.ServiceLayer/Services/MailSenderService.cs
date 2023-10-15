using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ProductionManagement.Exceptions;
using ProductionManagement.ServiceLayer.Interfaces;
using ProductionManagement.ServiceLayer.MailConfig;
using ProductionManagement.Models;

namespace ProductionManagement.ServiceLayer.Services
{
    public class MailSenderService : IMailSenderService
    {
        private readonly EmailConfiguration _settings;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContext;

        public MailSenderService(IOptions<EmailConfiguration> settings, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _settings = settings.Value;
            _userManager = userManager;
            _httpContext = httpContextAccessor;
        }

        public async Task<bool> SendAsync(Message message)
        {

            // Initialize a new instance of the MimeKit.MimeMessage class
            var mail = new MimeMessage();

            #region Sender / Receiver
            // Sender
            mail.From.Add(new MailboxAddress(_settings.DisplayName, message.From ?? _settings.From));
            mail.Sender = new MailboxAddress(message.DisplayName ?? _settings.DisplayName, message.From ?? _settings.From);

            // Receiver
            foreach (var mailAddress in message.To.Where(mailAddress => new EmailAddressAttribute().IsValid(mailAddress)))
            {
                mail.To.Add(MailboxAddress.Parse(mailAddress));
            }
            // Set Reply to if specified in mail data
            if (!string.IsNullOrEmpty(message.ReplyTo))
                mail.ReplyTo.Add(new MailboxAddress(message.ReplyToName, message.ReplyTo));

            // BCC
            // Check if a BCC was supplied in the request
            // Get only addresses where value is not null or with whitespace. x = value of address
            foreach (var mailAddress in message.Bcc.Where(x => !string.IsNullOrWhiteSpace(x)))
                mail.Bcc.Add(MailboxAddress.Parse(mailAddress.Trim()));

            // CC
            // Check if a CC address was supplied in the request
            foreach (var mailAddress in message.Cc.Where(x => !string.IsNullOrWhiteSpace(x)))
                mail.Cc.Add(MailboxAddress.Parse(mailAddress.Trim()));
            #endregion

            #region Content

            // Add Content to Mime Message
            var body = new BodyBuilder();
            mail.Subject = message.Subject;
            body.HtmlBody = message.Body;
            mail.Body = body.ToMessageBody();

            #endregion

            var result = await SendMail(mail);
            return result;
        }
        public async Task<bool> SendMail(MimeMessage mail, CancellationToken ct = default)
        {
            try
            {
                using var smtp = new SmtpClient();

                if (_settings.UseSSL)
                {
                    await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.SslOnConnect, ct);
                }
                else if (_settings.UseStartTls)
                {
                    await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, ct);
                }
                await smtp.AuthenticateAsync(_settings.UserName, _settings.Password, ct);
                await smtp.SendAsync(mail, ct);
                await smtp.DisconnectAsync(true, ct);
                return true;
            }

            catch (Exception)
            {
                return false;
            }

        }
        public async Task<bool> SendAccountInfoAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);

            // Initialize a new instance of the MimeKit.MimeMessage class
            var mail = new MimeMessage();
            var accountInfoMessage = new AccountInformationMessage(user, password);
            #region Sender / Receiver
            // Sender
            mail.From.Add(new MailboxAddress(_settings.DisplayName, _settings.From));
            mail.Sender = new MailboxAddress(_settings.DisplayName, _settings.From);

            // Receiver
            if (new EmailAddressAttribute().IsValid(accountInfoMessage.Email))
            {
                mail.To.Add(MailboxAddress.Parse(accountInfoMessage.Email));
            }

            // Add Content to Mime Message
            var body = new BodyBuilder();
            mail.Subject = accountInfoMessage.Subject;
            body.HtmlBody = accountInfoMessage.Body;
            mail.Body = body.ToMessageBody();

            #endregion
            var result = await SendMail(mail);
            return result;
        }
        public string GetAuthenticatedUsername()
        {
            return _httpContext.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name)?.Value
                ?? throw new RestrictedPermissionException("You are not authenticated or don't have permission");
        }
    }
}