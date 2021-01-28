using DomainModel.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace Repository
{
    public class UserTicketRepository : IUserTicketRepository
    {
        private readonly DatabaseContext _context;
        private readonly CustomIDataProtection _protector;
        public UserTicketRepository(DatabaseContext context, CustomIDataProtection protector)
        {
            _context = context;
            _protector = protector;
        }
        public void Add(UserTicket userTicket)
        {
            _context.UserTickets.Add(userTicket);
            _context.SaveChanges();
        }

        public void Delete(UserTicket userTicket)
        {
            _context.UserTickets.Remove(userTicket);
            _context.SaveChanges();
        }

        public void Edit(UserTicket userTicket)
        {
            _context.Entry(userTicket).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public UserTicket GetUserTicketById(int id)
        {
            return _context.UserTickets.Include(ut => ut.User).Include(ut => ut.Ticket).Include(ut => ut.Ticket.Event).FirstOrDefault(ut => ut.Id == id);
        }

        public IEnumerable<UserTicket> UserTicketList()
        {
            return _context.UserTickets.Include(ut => ut.User).Include(ut => ut.Ticket).Include(ut =>ut.Ticket.Event).ToList().Select(e => { e.EncryptedId = _protector.Encode(e.Id.ToString()); return e; });
        }
        public IEnumerable<UserTicket> UserTicketListByType(int id)
        {
            return _context.UserTickets.Include(ut => ut.User).Include(ut => ut.Ticket).Include(ut => ut.Ticket.Event).ToList().Where(ut => ut.TicketId == id).Select(e => { e.EncryptedId = _protector.Encode(e.Id.ToString()); return e; });
        }
    }
}
