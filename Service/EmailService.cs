using DomainModel.Entities;
using MailKit.Net.Smtp;
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
        public void SendQRCode(UserTicket userTicket)
        {
            var mimemessage = new MimeMessage();

            mimemessage.From.Add(new MailboxAddress("Your name", "YourMail@gmail.com"));
            mimemessage.To.Add(MailboxAddress.Parse(userTicket.User.Email));
            mimemessage.Subject = "Thank you for Purchase Ticket! This is your QR Code Ticket.";
            var builder = new BodyBuilder();
            var image = userTicket.QRCode;
            var qr = builder.LinkedResources.Add("image", image);
            qr.ContentId = MimeUtils.GenerateMessageId();
            builder.HtmlBody = string.Format(@"<center><img src=""cid:{0}""></center>",qr.ContentId);
            mimemessage.Body = builder.ToMessageBody();

            var client = new SmtpClient();
            client.Connect("smtp.gmail.com", 587);
            ////Note: only needed if the SMTP server requires authentication
            client.Authenticate("YourMail@gmail.com", "YourMailPassword");
            client.Send(mimemessage);
            client.Disconnect(true);
        }
    }
}
