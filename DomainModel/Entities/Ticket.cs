using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DomainModel.Entities
{
    public class Ticket : Entity
    {
        [Display(Name ="Ticket name")]
        public string TicketName { get; set; }
        public string Description { get; set; }
        [Display(Name = "Expiry Date")]
        public DateTimeOffset ExpiryDate { get; set; }
        public int Quantity { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public ICollection<UserTicket> UserTickets { get; set; }
    }
}
