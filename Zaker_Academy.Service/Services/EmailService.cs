﻿using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public EmailService(IOptions<EmailHelper> options )
        {
            emailHelper = options.Value;
        }

        public async Task<ServiceResult> sendAsync(string Subject, string body, string Sender, string reciver)
        {
            try
            {
                var Email = new MimeMessage();
                Email.From.Add(MailboxAddress.Parse(Sender));
                Email.To.Add(MailboxAddress.Parse(reciver));
                Email.Subject = Subject;
                Email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };
                using (var Smtp = new SmtpClient())
                {
                    await Smtp.ConnectAsync(emailHelper.Host, emailHelper.Port, SecureSocketOptions.StartTls);
                    await Smtp.AuthenticateAsync(Sender,emailHelper.Password);
                    await Smtp.SendAsync(Email);
                    await Smtp.DisconnectAsync(true);
                }
                return new ServiceResult() { succeeded = true };
            }
            catch (Exception e)
            {
                return new ServiceResult() { Message = "Verification Email Process Failed ", Details = e };
            }
        }
    }
}