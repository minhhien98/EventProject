using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Interface
{
    public interface IEventRepository
    {
        void Add(Event ev);
        void Edit(Event ev);
        void Delete(Event ev);
        IEnumerable<Event> EventList();
        Event GetEventById(int id);
    }
}
