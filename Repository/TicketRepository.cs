using DomainModel.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class TicketRepository : ITicketRepository
    {
        private readonly DatabaseContext context;
        public TicketRepository()
        {
            context = new DatabaseContext();
        }
        public void Add(Ticket ticket)
        {
            context.Tickets.Add(ticket);
            context.SaveChanges();
        }

        public void Delete(Ticket ticket)
        {
            context.Tickets.Remove(ticket);
            context.SaveChanges();
        }

        public void Edit(Ticket ticket)
        {
            context.Entry(ticket).State = EntityState.Modified;
            context.SaveChanges();
        }

        public Ticket GetTicketById(int id)
        {
            return context.Tickets.Include(t => t.Event).FirstOrDefault(t=>t.Id == id);
        }

        public IEnumerable<Ticket> TicketList()
        {
            return context.Tickets.Include(t => t.Event).ToList();
        }
    }
}
