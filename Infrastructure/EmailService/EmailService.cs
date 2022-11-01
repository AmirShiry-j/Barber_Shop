using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.EmailService
{
    public interface IEmailService
    {
        Task<bool> SendEmail(string UserEmail, string Body, string Subject);
    }
    public class EmailService : IEmailService
    {
        IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        public EmailService(ILogger<EmailService> logger,IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<bool> SendEmail(string UserEmail, string Body, string Subject)
        {
            //enable less secure apps in account google with link
            //https://myaccount.google.com/lesssecureapps
            //https://mail.google.com/mail/u/0/?tab=km#inbox

            try
            {
                string email = _configuration["EmailSetting:Email"];
                string password = _configuration["EmailSetting:Password"];

                SmtpClient client = new SmtpClient();
                client.Port = int.Parse(_configuration["EmailSetting:Port"]);
                client.Host = _configuration["EmailSetting:Host"];
                client.EnableSsl = bool.Parse(_configuration["EmailSetting:EnableSsl"]);
                client.Timeout = int.Parse(_configuration["EmailSetting:Timeout"]);
                client.UseDefaultCredentials = bool.Parse(_configuration["EmailSetting:UseDefaultCredentials"]);

                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential(email, password);

                MailMessage message = new MailMessage(email, UserEmail, Subject, Body);
                message.IsBodyHtml = true;
                message.BodyEncoding = UTF8Encoding.UTF8;
                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

                client.Send(message);

                return true;
            }
            catch (Exception error)
            {
                //Log error
                _logger.LogError(error.ToString());

                return false;
            }
        }
    }
}
