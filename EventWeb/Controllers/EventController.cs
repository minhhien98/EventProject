using AutoMapper;
using DomainModel.Entities;
using EventWeb.Models.ViewModel.EventVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventWeb.Controllers
{   
    public class EventController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;
        private readonly ITicketService _ticketService;
        public EventController(IMapper mapper,IEventService eventService,ITicketService ticketService)
        {
            _mapper = mapper;
            _eventService = eventService;
            _ticketService = ticketService;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(EventVM vm)
        {
            if (ModelState.IsValid)
            {
                if(vm.Date < DateTimeOffset.UtcNow)
                {
                    ModelState.AddModelError("Date", "Invalid Date.");
                    return View(vm);
                }
                var ev = _mapper.Map<Event>(vm);
                ev.Date = vm.Date.ToUniversalTime();
                _eventService.AddEvent(ev);
                var ticket = new Ticket()
                {
                    TicketName = ev.EventName + " Ticket",
                    Description = ev.Description,
                    ExpiryDate = ev.Date.AddDays(1),
                    Quantity = vm.Quantity,
                    EventId = ev.Id
                };
                _ticketService.AddTicket(ticket);             
                return RedirectToAction("Index", "Home");
            }
            return View(vm);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var ticket =_ticketService.GetTicketById(id);
            var vm = new EventVM()
            {
                EventId = ticket.Event.Id,
                EventName = ticket.Event.EventName,
                Date = ticket.Event.Date.LocalDateTime,
                Description = ticket.Event.Description,
                Quantity = ticket.Quantity,
            };
            return View(vm);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(EventVM vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.Date < DateTimeOffset.UtcNow)
                {
                    ModelState.AddModelError("Date", "Invalid Date.");
                    return View(vm);
                }
                var ev = _mapper.Map<Event>(vm);
                ev.Date = vm.Date.ToUniversalTime();
                _eventService.EditEvent(ev);

                var ticket = _ticketService.GetTicketByEventId(vm.EventId);
                ticket.TicketName = ev.EventName + " Ticket";
                ticket.Description = ev.Description;
                ticket.ExpiryDate = ev.Date.AddDays(1);
                ticket.Quantity = vm.Quantity;
                _ticketService.EditTicket(ticket);
                
                return RedirectToAction("Index", "Home");
            }
            return View(vm);
        }
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public IActionResult Delete(int EventId)
        {
            var ev = _eventService.GetEventById(EventId);
            _eventService.DeleteEvent(ev);
            return RedirectToAction("Index", "Home");
        }
    }
}
