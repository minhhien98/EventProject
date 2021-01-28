using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel.Entities
{
    public class Ticket : Entity
    {
        public string TicketName { get; set; }
        public string Description { get; set; }
        public DateTimeOffset ExpiryDate { get; set; }
        public int Quantity { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public ICollection<UserTicket> UserTickets { get; set; }
    }
}
