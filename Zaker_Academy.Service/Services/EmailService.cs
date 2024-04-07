using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Zaker_Academy.Service.ErrorHandling;
using Zaker_Academy.Service.Helper;
using Zaker_Academy.Service.Interfaces;

namespace Zaker_Academy.Service.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailHelper emailHelper;
        private readonly IConfiguration Configuration;
        public EmailService(IOptions<EmailHelper> options, IConfiguration configuration)
        {
            emailHelper = options.Value;
            Configuration = configuration;
        }



        public async Task<ServiceResult<string>> sendAsync(string Subject, string body, string Sender, string reciver)
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
                if (response.IsSuccessStatusCode)
                    return new ServiceResult<string>() {  succeeded = true };
                throw new Exception(message: "Verfication Failed !! , Please Try again later.");

            }
            catch (Exception e)
            {
                throw new Exception(message: e.Message);
            }
        }
    }
}