using DomainModel.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class UserTicketRepository : IUserTicketRepository
    {
        private readonly DatabaseContext context;
        public UserTicketRepository()
        {
            context = new DatabaseContext();
        }
        public void Add(UserTicket userTicket)
        {
            context.UserTickets.Add(userTicket);
            context.SaveChanges();
        }

        public void Delete(UserTicket userTicket)
        {
            context.UserTickets.Remove(userTicket);
            context.SaveChanges();
        }

        public void Edit(UserTicket userTicket)
        {
            context.Entry(userTicket).State = EntityState.Modified;
            context.SaveChanges();
        }

        public UserTicket GetUserTicketById(int id)
        {
            return context.UserTickets.Include(ut => ut.User).Include(ut => ut.Ticket).FirstOrDefault(ut => ut.Id == id);
        }

        public IEnumerable<UserTicket> UserTicketList()
        {
            return context.UserTickets.Include(ut => ut.User).Include(ut => ut.Ticket).ToList();
        }
        public IEnumerable<UserTicket> UserTicketListByType(int id)
        {
            return context.UserTickets.Include(ut => ut.User).Include(ut => ut.Ticket).ToList().Where(ut => ut.TicketId == id);
        }
    }
}
