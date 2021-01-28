using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel.Entities
{
    public class UserTicket : Entity
    {
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTimeOffset PurchaseDate { get; set; }
        public byte[] QRCode { get; set; }

    }
}
