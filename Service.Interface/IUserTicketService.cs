using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IUserTicketService
    {
        void AddUserTicket(UserTicket userTicket);
        void EditUserTicket(UserTicket userTicket);
        void DeleteTicket(UserTicket userTicket);
        IEnumerable<UserTicket> GetUserTicketList();
        UserTicket GetUserTicketById(int id);
        IEnumerable<UserTicket> GetUserTicketListByType(int id);
    }
}
