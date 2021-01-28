using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface IEventService
    {
        void AddEvent(Event ev);
        void EditEvent(Event ev);
        void DeleteEvent(Event ev);
        IEnumerable<Event> GetEventList();
        Event GetEventById(int id);

    }
}
