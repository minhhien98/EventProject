using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Interface
{
    public interface IUserTicketRepository
    {
        void Add(UserTicket userTicket);
        void Edit(UserTicket userTicket);
        void Delete(UserTicket userTicket);
        IEnumerable<UserTicket> UserTicketList();
        UserTicket GetUserTicketById(int id);
        IEnumerable<UserTicket> UserTicketListByType(int id);
    }
}
