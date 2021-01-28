using EventWeb.Models.ViewModel.TicketVM;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace EventWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly IUserService _userService;
        private readonly CustomIDataProtection _protector;
        public HomeController(ITicketService ticketService,IUserService userService,CustomIDataProtection protector)
        {
            _ticketService = ticketService;
            _userService = userService;
            _protector = protector;
        }
        public IActionResult Index()
        {
            var user = _userService.GetUserByUserName(User.Identity.Name);
            var vm = new TicketListVM()
            {
                Tickets = _ticketService.GetTicketList().Where(e => e.ExpiryDate > DateTimeOffset.UtcNow),
                CurrentUser = user
            };
            return View(vm);
        }
    }
}
