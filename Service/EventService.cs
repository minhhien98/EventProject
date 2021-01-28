using DomainModel.Entities;
using Repository;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public class EventService : IEventService
    {
        private readonly IEventRepository eventRepository;
        public EventService()
        {
            eventRepository = new EventRepository();
        }
        public void AddEvent(Event ev)
        {
            eventRepository.Add(ev);
        }

        public void DeleteEvent(Event ev)
        {
            eventRepository.Delete(ev);
        }

        public void EditEvent(Event ev)
        {
            eventRepository.Edit(ev);
        }

        public Event GetEventById(int id)
        {
            return eventRepository.GetEventById(id);
        }

        public IEnumerable<Event> GetEventList()
        {
            return eventRepository.EventList();
        }
    }
}
