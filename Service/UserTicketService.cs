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
        private readonly IUserTicketRepository _utRepository;
        public UserTicketService(IUserTicketRepository utRepository)
        {
            _utRepository = utRepository;
        }
        public void AddUserTicket(UserTicket userTicket)
        {            
            _utRepository.Add(userTicket);          
        }

        public void DeleteTicket(UserTicket userTicket)
        {
            _utRepository.Delete(userTicket);
        }

        public void EditUserTicket(UserTicket userTicket)
        {
            _utRepository.Edit(userTicket);
        }

        public UserTicket GetUserTicketById(int id)
        {
            return _utRepository.GetUserTicketById(id);
        }

        public IEnumerable<UserTicket> GetUserTicketList()
        {
            return _utRepository.UserTicketList();
        }

        public IEnumerable<UserTicket> GetUserTicketListByType(int id)
        {
            return _utRepository.UserTicketListByType(id);
        }
    }
}
