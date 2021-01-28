using DomainModel.Entities;
using MailKit.Net.Smtp;
using MimeKit;
using QRCoder;
using Repository;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using Utility;

namespace Service
{
    public class UserTicketService : IUserTicketService
    {
        private readonly IUserTicketRepository utr;
        private readonly IEmailService emailService;
        public UserTicketService()
        {
            utr = new UserTicketRepository();
            emailService = new EmailService();
        }
        public bool AddUserTicket(UserTicket userTicket)
        {
            var TicketCount = GetUserTicketListByType(userTicket.TicketId).Count();
            if(userTicket.Ticket.Quantity < TicketCount)
            {
                userTicket.PurchaseDate = DateTimeOffset.UtcNow;               
                userTicket = QRGenerator.QRImageGen(userTicket); //Generate QR Image
                utr.Add(userTicket);
                emailService.SendQRCode(userTicket);            
                return true;
            }
            return false;
        }

        public void DeleteTicket(UserTicket userTicket)
        {
            utr.Delete(userTicket);
        }

        public void EditUserTicket(UserTicket userTicket)
        {
            utr.Edit(userTicket);
        }

        public UserTicket GetUserTicketById(int id)
        {
            return utr.GetUserTicketById(id);
        }

        public IEnumerable<UserTicket> GetUserTicketList()
        {
            return utr.UserTicketList();
        }

        public IEnumerable<UserTicket> GetUserTicketListByType(int id)
        {
            return utr.UserTicketListByType(id);
        }
    }
}
