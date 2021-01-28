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
        private readonly ITicketRepository _ticketRepository;
        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }
        public void AddTicket(Ticket ticket)
        {
            _ticketRepository.Add(ticket);
        }

        public void DeleteTicket(Ticket ticket)
        {
            _ticketRepository.Delete(ticket);
        }

        public void EditTicket(Ticket ticket)
        {
            _ticketRepository.Edit(ticket);
        }

        public Ticket GetTicketById(int id)
        {
            return _ticketRepository.GetTicketById(id);
        }

        public IEnumerable<Ticket> GetTicketList()
        {
            return _ticketRepository.TicketList();
        }
        public Ticket GetTicketByEventId(int id)
        {
            return _ticketRepository.GetTicketByEventId(id);
        }
    }
}
