﻿using DomainModel.Entities;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace EventWeb.Controllers
{
    public class UserTicketController : Controller
    {

        private readonly ITicketService _ticketService;
        private readonly IUserTicketService _userTicketService;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly CustomIDataProtection _protector;
        public UserTicketController(ITicketService ticketService, IUserTicketService userTicketService, IUserService userService, IEmailService emailService, CustomIDataProtection protector)
        {
            _ticketService = ticketService;
            _userTicketService = userTicketService;
            _userService = userService;
            _emailService = emailService;
            _protector = protector;
        }
        private string GenerateQRCode()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[30];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }
        [Authorize]
        [HttpPost]
        public IActionResult PurchaseTicket(int TicketId)
        {
            var ticket = _ticketService.GetTicketById(TicketId);
            if(ticket == null || ticket.ExpiryDate < DateTimeOffset.UtcNow)
            {
                TempData["PurchaseStatus"] = "Ticket is not available.";
                return RedirectToAction("Index", "Home");
            }
            if (ticket.Quantity == ticket.UserTickets.Count)
            {
                TempData["PurchaseStatus"] = ticket.TicketName +" is sold out.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var user = _userService.GetUserByUserName(User.Identity.Name);
                var qrCode = GenerateQRCode();
                while(_userTicketService.GetUserTicketList().Where(ut=>ut.QRCode == qrCode).Any())
                {
                    qrCode = GenerateQRCode();
                }
                var userTicket = new UserTicket()
                {
                    TicketId = TicketId,
                    UserId = user.Id,
                    PurchaseDate = DateTimeOffset.UtcNow,
                    QRCode = qrCode,
                };                
                _userTicketService.AddUserTicket(userTicket);
                //string url = Url.Action("CheckIn", "UserTicket");
                //string url = "192.168.1.2:45455/userticket/checkin"; // Testing with 'conveyor by keyoti' extension. Edit IP to your IPv4 in cmd.exe ipconfig
                var image = QRGenerator.QRImageGen(ticket.TicketName,user.LastName+" "+user.FirstName,userTicket.QRCode);
                //_userTicketService.EditUserTicket(userTicket);
                _emailService.SendQRCode(user.Email,ticket.TicketName,image);
                TempData["PurchaseStatus"] = "Purchase Successfully.";
                return RedirectToAction("Index", "Home");
            }           
        }
        [Authorize]
        public IActionResult MyTicket()
        {
            var user = _userService.GetUserByUserName(User.Identity.Name);
            if(user == null)
            {
                return NotFound();
            }
            var list = _userTicketService.GetUserTicketList().Where(u => u.UserId == user.Id).OrderByDescending(u=>u.PurchaseDate);
            return View(list);
        }
        [Authorize]
        public IActionResult ResendQrTicket(string id)
        {
            var user = _userService.GetUserByUserName(User.Identity.Name);
            var ut = _userTicketService.GetUserTicketById(Convert.ToInt32(_protector.Decode(id)));
            if(ut != null && user != null)
            {
                var image = QRGenerator.QRImageGen(ut.Ticket.TicketName, user.LastName + " " + user.FirstName, ut.QRCode);
                _emailService.SendQRCode(user.Email, ut.Ticket.TicketName, image);
                TempData["TicketStatus"] = "Ticket has been resent. Please check your email.";
                return RedirectToAction("MyTicket", "UserTicket");
            }
            TempData["TicketStatus"] = "Something wrong, please try again.";
            return RedirectToAction("MyTicket", "UserTicket");
        }

        [Authorize(Roles ="Admin")]
        public IActionResult UTList()
        {
            var list = _userTicketService.GetUserTicketList();
            return View(list);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var ut = _userTicketService.GetUserTicketById(id);
            if (ut != null)
            {
                _userTicketService.DeleteTicket(ut);
                return RedirectToAction("UTList", "UserTicket");
            }
            return NotFound();
        }
    }
}
