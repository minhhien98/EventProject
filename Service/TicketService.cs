using DomainModel.Entities;
using Repository;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository ticketRepository;
        public TicketService()
        {
            ticketRepository = new TicketRepository();
        }
        public void AddTicket(Ticket ticket)
        {
            ticketRepository.Add(ticket);
        }

        public void DeleteTicket(Ticket ticket)
        {
            ticketRepository.Delete(ticket);
        }

        public void EditTicket(Ticket ticket)
        {
            ticketRepository.Edit(ticket);
        }

        public Ticket GetTicketById(int id)
        {
            return ticketRepository.GetTicketById(id);
        }

        public IEnumerable<Ticket> GetTicketList()
        {
            return ticketRepository.TicketList();
        }
    }
}
