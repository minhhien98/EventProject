using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventWeb.Models.ViewModel.TicketVM
{
    public class TicketListVM
    {
        public IEnumerable<Ticket> Tickets { get; set; }
        public User CurrentUser { get; set; }
    }
}
