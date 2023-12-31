﻿using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zaker_Academy.Service.ErrorHandling;
using Zaker_Academy.Service.Interfaces;

namespace Zaker_Academy.Service.Services
{
    public class EmailService : IEmailService
    {
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
                    await Smtp.ConnectAsync("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
                    await Smtp.AuthenticateAsync(Sender, "ZMD1ggBpVPNNqbZAT6");
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