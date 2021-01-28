using DomainModel.Entities;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Utils;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public class EmailService : IEmailService
    {       
        public class MailSettings
        {
            public string Mail { get; set; }
            public string DisplayName { get; set; }
            public string Password { get; set; }
            public string Host { get; set; }
            public int Port { get; set; }
        }

        private readonly MailSettings _mailSettings;
        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public void SendQRCode(string Email,string TicketName,byte[] QRCode)
        {
            var mimemessage = new MimeMessage();

            mimemessage.From.Add(new MailboxAddress(_mailSettings.DisplayName, "YourMail@Service.com"));
            mimemessage.To.Add(MailboxAddress.Parse(Email));
            mimemessage.Subject = "Thank you for Purchasing Ticket! This is your "+TicketName + " QR Code.";
            var builder = new BodyBuilder();
            var qr = builder.LinkedResources.Add("image", QRCode);
            qr.ContentId = MimeUtils.GenerateMessageId();
            builder.HtmlBody = string.Format(@"<center><img width=""300"" weight=""300"" src=""cid:{0}""></center>",qr.ContentId);
            mimemessage.Body = builder.ToMessageBody();
            Send(mimemessage);
        }

        public void SendEmail(string Email,string Subject,string Content)
        {
            var mimemessage = new MimeMessage();

            mimemessage.From.Add(new MailboxAddress(_mailSettings.DisplayName, "YourMail@Service.com"));
            mimemessage.To.Add(MailboxAddress.Parse(Email));
            mimemessage.Subject = Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = Content;
            mimemessage.Body = builder.ToMessageBody();
            Send(mimemessage);
            
        }

        void Send(MimeMessage mimemessage)
        {
            var client = new SmtpClient();
            client.Connect(_mailSettings.Host, _mailSettings.Port);
            ////Note: only needed if the SMTP server requires authentication
            client.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            client.Send(mimemessage);
            client.Disconnect(true);
        }
    }
}
