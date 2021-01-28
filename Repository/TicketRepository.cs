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
        private readonly DatabaseContext _context;
        public TicketRepository(DatabaseContext context)
        {
            _context = context;
        }
        public void Add(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            _context.SaveChanges();
        }

        public void Delete(Ticket ticket)
        {
            _context.Tickets.Remove(ticket);
            _context.SaveChanges();
        }

        public void Edit(Ticket ticket)
        {
            _context.Entry(ticket).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public Ticket GetTicketByEventId(int id)
        {
            return _context.Tickets.FirstOrDefault(t=>t.EventId == id);
        }

        public Ticket GetTicketById(int id)
        {
            return _context.Tickets.Include(t => t.Event).Include(t => t.UserTickets).FirstOrDefault(t=>t.Id == id);
        }

        public IEnumerable<Ticket> TicketList()
        {
            return _context.Tickets.Include(t => t.Event).Include(t=>t.UserTickets).ToList();
        }
    }
}
