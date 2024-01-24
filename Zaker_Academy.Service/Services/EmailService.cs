using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Zaker_Academy.Service.ErrorHandling;
using Zaker_Academy.Service.Helper;
using Zaker_Academy.Service.Interfaces;

namespace Zaker_Academy.Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailHelper emailHelper;
        private readonly IConfiguration Configuration;
        public EmailService(IOptions<EmailHelper> options ,IConfiguration configuration  )
        {
            emailHelper = options.Value;
            Configuration = configuration;
        }

     

        public async Task<ServiceResult> sendAsync(string Subject, string body, string Sender, string reciver)
        {
            try
            {
                var apiKey = Configuration.GetSection("SendGridAPIKey").Value;
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(emailHelper.Address, Sender);
                var subject = Subject;
                var to = new EmailAddress(reciver, reciver);
                var plainTextContent = "and easy to do anywhere, even with C#";
                var htmlContent = body;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
                //var Email = new MimeMessage();
                //Email.From.Add(MailboxAddress.Parse(Sender));
                //Email.To.Add(MailboxAddress.Parse(reciver));
                //Email.Subject = Subject;
                //Email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };
                //using (var Smtp = new SmtpClient())
                //{
                //    await Smtp.ConnectAsync(emailHelper.Host, emailHelper.Port, SecureSocketOptions.StartTls);
                //    await Smtp.AuthenticateAsync(Sender,emailHelper.Password);
                //    await Smtp.SendAsync(Email);
                //    await Smtp.DisconnectAsync(true);
                //}
                return new ServiceResult() { succeeded = true };
            }
            catch (Exception e)
            {
                return new ServiceResult() { Message = "Verification Email Process Failed ", Details = e };
            }
        }
    }
}