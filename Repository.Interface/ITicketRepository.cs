using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Interface
{
    public interface ITicketRepository
    {
        void Add(Ticket ticket);
        void Edit(Ticket ticket);
        void Delete(Ticket ticket);
        IEnumerable<Ticket> TicketList();
        Ticket GetTicketById(int id);
    }
}
