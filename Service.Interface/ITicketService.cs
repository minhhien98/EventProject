using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface ITicketService
    {
        void AddTicket(Ticket ticket);
        void EditTicket(Ticket ticket);
        void DeleteTicket(Ticket ticket);
        IEnumerable<Ticket> GetTicketList();
        Ticket GetTicketById(int id);
    }
}
