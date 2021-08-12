﻿//using MailKit.Security;
using Microsoft.Extensions.Options;

using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using MailKit.Security;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using System.IO;

namespace OnlineShop.Models.Email
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            // _mailSettings.Mail = new MailAddress(_mailSettings.Mail, "App Admin");
          
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            //----------------------
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            //----------------------
  
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            MailboxAddress from = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
            email.From.Add(from);
            using (var smtp = new SmtpClient())
            {
                smtp.Connect(_mailSettings.Host, _mailSettings.Port,SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
        
        }
    }
}
