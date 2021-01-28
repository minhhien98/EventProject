using DomainModel.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class EventRepository : IEventRepository
    {
        private readonly DatabaseContext context;
        public EventRepository()
        {
            context = new DatabaseContext();
        }
        public void Add(Event ev)
        {
            context.Events.Add(ev);
            context.SaveChanges();
        }

        public void Delete(Event ev)
        {
            context.Events.Remove(ev);
            context.SaveChanges();
        }

        public void Edit(Event ev)
        {
            context.Entry(ev).State = EntityState.Modified;
            context.SaveChanges();
        }

        public IEnumerable<Event> EventList()
        {
            return context.Events.ToList();
        }

        public Event GetEventById(int id)
        {
            return context.Events.Find(id);
        }
    }
}
