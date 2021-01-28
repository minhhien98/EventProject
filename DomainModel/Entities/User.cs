using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DomainModel.Entities
{
    public class User : Entity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string Email { get; set; }
        public string NewEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string EmailCode { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public ICollection<UserTicket> userTickets { get;set; }
    }
}
