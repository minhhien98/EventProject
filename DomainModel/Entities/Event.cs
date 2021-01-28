using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel.Entities
{
    public class Event : Entity
    {
        public string EventName { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
