using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface IEmailService
    {
        void SendQRCode(string Email, string TicketName, byte[] QRCode);
        void SendEmail(string Email, string Subject,string Content);
    }
}
