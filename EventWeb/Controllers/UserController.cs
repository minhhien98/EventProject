using AutoMapper;
using DomainModel.Entities;
using EventWeb.Models.ViewModel.UserVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Interface;
using System;
using System.IO;
using System.Linq;
using Utility;

namespace EventWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;
        private readonly CustomIDataProtection _protector;
        public UserController(IUserService userService,IEmailService emailService,IMapper mapper,IRoleService roleService, CustomIDataProtection protector)
        {
            _userService = userService;
            _emailService = emailService;
            _mapper = mapper;
            _roleService = roleService;
            _protector = protector;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(UserVM vm)
        {
            if (ModelState.IsValid)
            {
                var ExistUser = _userService.GetUserByUserName(vm.Username);
                if (ExistUser != null)
                {
                    ModelState.AddModelError("Username", "This Username already taken.");
                    return View(vm);
                }
                if(vm.Birthday >= DateTime.UtcNow)
                {
                    ModelState.AddModelError("Birthday", "Invalid Date.");
                    return View(vm);
                }
                vm.RoleId = 2;
                var user = _mapper.Map<User>(vm);
                _userService.AddUser(user);                
                return RedirectToAction("SendConfirmMail", "User",new { userId = _protector.Encode(user.Id.ToString()), email = _protector.Encode(user.Email)});
            }
            return View(vm);
        } 
        [Authorize]
        public IActionResult ChangeInfo()
        {
            var user = _userService.GetUserByUserName(User.Identity.Name);
            if(user != null)
            {
                var vm = _mapper.Map<UserVM>(user);
                return View(vm);
            }
            return NotFound();
        }
        [Authorize]
        [HttpPost]
        public IActionResult ChangeInfo(UserVM vm)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(vm);
                _userService.EditUser(user);
                ModelState.AddModelError("", "Your Info have been changed");
                return View(vm);
            }
            return View(vm);
        }
        public IActionResult SendConfirmMail(string userId,string email)
        {
            var vm = new EmailConfirmationVM()
            {
                UserId = Convert.ToInt32(_protector.Decode(userId)),
                Email = _protector.Decode(email)
            };
            if (!IsValidEmail(vm.Email))
            {
                ModelState.AddModelError("Email", "Invalid Email");
                return View(vm);
            }
            //send Confirm Email
            var user = _userService.GetUserById(vm.UserId);
            if(user == null)
            {
                return NotFound();
            }
            string code = Guid.NewGuid().ToString();
            user.EmailCode = code;
            _userService.EditUser(user);
            var subject = "Email Confirmation.";
            var CallBackUrl = Url.Action("ConfirmEmail", "User", new { userId = userId, code = _protector.Encode(code) }, protocol: Request.Scheme);
            var mailTemplate = System.IO.File.ReadAllText("./Models/ViewModel/UserVM/EmailConfirm.html");
            mailTemplate = mailTemplate.Replace("{UserName}", user.UserName);
            mailTemplate = mailTemplate.Replace("{Content}", "Please click bellow button for confirm your email.");
            mailTemplate = mailTemplate.Replace("{ConfirmationLink}", CallBackUrl);
            _emailService.SendEmail(user.Email, subject, mailTemplate);
            ModelState.AddModelError("", "Confirmation mail is sent. Please check your email.");
            return View(vm);

        }
        [HttpPost]
        public IActionResult SendConfirmMail(EmailConfirmationVM vm)
        {
            
            if (ModelState.IsValid)
            {
                var user = _userService.GetUserById(vm.UserId);
                if (vm.Email != user.Email)
                {
                    user.Email = vm.Email;
                    _userService.EditUser(user);
                }
                //send Confirm Email
                string code = Guid.NewGuid().ToString();
                user.EmailCode = code;
                _userService.EditUser(user);
                var subject = "Email Confirmation.";
                var CallBackUrl = Url.Action("ConfirmEmail", "User", new { userId = _protector.Encode(user.Id.ToString()), code = _protector.Encode(code) }, protocol: Request.Scheme);
                var mailTemplate = System.IO.File.ReadAllText("./Models/ViewModel/UserVM/EmailConfirm.html");
                mailTemplate = mailTemplate.Replace("{UserName}", user.UserName);
                mailTemplate = mailTemplate.Replace("{Content}", "Please click bellow button for confirm your email.");
                mailTemplate = mailTemplate.Replace("{ConfirmationLink}", CallBackUrl);
                _emailService.SendEmail(user.Email,subject ,mailTemplate);
                ModelState.AddModelError("", "Confirmation mail is sent. Please check your email.");
                return View(vm);
            }
            return View(vm);
        }
        public IActionResult ConfirmEmail(string userId,string code)
        {
            var user = _userService.GetUserById(Convert.ToInt32(_protector.Decode(userId)));
            if (user != null && user.EmailConfirmed == false && user.EmailCode == _protector.Decode(code))
            {
                user.EmailConfirmed = true;
                user.EmailCode = "";
                _userService.EditUser(user);
                return View();
            }
            return NotFound();
        }
        [Authorize]
        public IActionResult ChangeEmail()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult ChangeEmail(ChangeEmailVM vm)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetUserByUserName(User.Identity.Name);
                if(user.Email != vm.OldEmail)
                {
                    ModelState.AddModelError("OldEmail", "Your current email is not correct.");
                    return View(vm);
                }
                string code = Guid.NewGuid().ToString();
                user.EmailCode = code;
                user.NewEmail = vm.NewEmail;
                _userService.EditUser(user);
                var subject = "Your Email is being changed!";
                var CallBackUrl = Url.Action("ConfirmChangeEmail", "User", new { userId = _protector.Encode(user.Id.ToString()), code = _protector.Encode(code) }, protocol: Request.Scheme);
                var mailTemplate = System.IO.File.ReadAllText("./Models/ViewModel/UserVM/EmailConfirm.html");
                mailTemplate = mailTemplate.Replace("{UserName}", user.UserName);
                mailTemplate = mailTemplate.Replace("{Content}", "Please click bellow button for confirm your changing email process.");
                mailTemplate = mailTemplate.Replace("{ConfirmationLink}", CallBackUrl);
                _emailService.SendEmail(user.Email, subject, mailTemplate);
                ModelState.AddModelError("", "Change email In process. Please Check email to confirm.");
                return View(vm);
            }
            return View(vm);
        }
        public IActionResult ConfirmChangeEmail(string userId,string code)
        {
            var user = _userService.GetUserById(Convert.ToInt32(_protector.Decode(userId)));
            if (user != null && user.EmailConfirmed == true && user.EmailCode == _protector.Decode(code))
            {
                code = Guid.NewGuid().ToString();
                user.EmailCode = code;
                _userService.EditUser(user);
                var subject = "Confirm New Email!";
                var CallBackUrl = Url.Action("ConfirmNewEmail", "User", new { userId = userId, code = _protector.Encode(code) }, protocol: Request.Scheme);
                var mailTemplate = System.IO.File.ReadAllText("./Models/ViewModel/UserVM/EmailConfirm.html");
                mailTemplate = mailTemplate.Replace("{UserName}", user.UserName);
                mailTemplate = mailTemplate.Replace("{Content}", "Please click bellow button for confirm your new Email.");
                mailTemplate = mailTemplate.Replace("{ConfirmationLink}", CallBackUrl);
                _emailService.SendEmail(user.NewEmail, subject, mailTemplate);
                return View();
            }
            return NotFound();
        }
        public IActionResult ConfirmNewEmail(string userId,string code)
        {
            var user = _userService.GetUserById(Convert.ToInt32(_protector.Decode(userId)));
            if (user != null && user.EmailConfirmed == true && user.EmailCode == _protector.Decode(code))
            {
                user.Email = user.NewEmail;
                user.NewEmail = "";
                _userService.EditUser(user);
                return View();
            }
            return NotFound();
        }
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordVM vm)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetUserByUserName(User.Identity.Name);
                if (user == null)
                {
                    return NotFound();
                }
                if(user.Password != vm.OldPassword)
                {
                    ModelState.AddModelError("OldPassword", "Invalid Password");
                    return View(vm);
                }
                if(vm.OldPassword == vm.NewPassword)
                {
                    ModelState.AddModelError("NewPassword", "Please use new password");
                    return View(vm);
                }
                var code = Guid.NewGuid().ToString();
                user.NewPassword = vm.NewPassword;
                user.EmailCode = code;
                _userService.EditUser(user);
                var subject = "Your Password is being changed!";
                var CallBackUrl = Url.Action("ConfirmChangePassword", "User", new { userId = _protector.Encode(user.Id.ToString()), code = _protector.Encode(code) }, protocol: Request.Scheme);
                var mailTemplate = System.IO.File.ReadAllText("./Models/ViewModel/UserVM/EmailConfirm.html");
                mailTemplate = mailTemplate.Replace("{UserName}", user.UserName);
                mailTemplate = mailTemplate.Replace("{Content}", "Please click bellow button to change password.");
                mailTemplate = mailTemplate.Replace("{ConfirmationLink}", CallBackUrl);
                _emailService.SendEmail(user.Email, subject, mailTemplate);
                ModelState.AddModelError("", "Please check your email for changing password.");
                return View(vm);
            }            
            return View(vm);
        }
        public IActionResult ConfirmChangePassword(string userId,string code)
        {
            var user = _userService.GetUserById(Convert.ToInt32(_protector.Decode(userId)));
            if (user != null && user.EmailConfirmed == true && user.EmailCode == _protector.Decode(code))
            {
                user.Password = user.NewPassword;
                user.NewPassword = "";
                user.EmailCode = "";
                _userService.EditUser(user);
                return View();
            }
            return NotFound();
        }


        [Authorize(Roles ="Admin")]
        public IActionResult UserList()
        {
            var list = _userService.GetUserList();
            return View(list);
        }
        [Authorize(Roles ="Admin")]
        public IActionResult Edit(string id)
        {
            var user = _userService.GetUserById(Convert.ToInt32(_protector.Decode(id)));
            if (user != null)
            {
                var vm = _mapper.Map<UserVM>(user);
                vm.roleList = _roleService.GetRoleList().Select(r=> new SelectListItem() { Value = r.Id.ToString(), Text = r.RoleName }).ToList();
                return View(vm);
            }
            return NotFound();
        }
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public IActionResult Edit(UserVM vm)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(vm);
                _userService.EditUser(user);
                return RedirectToAction("UserList", "User");
            }
            vm.roleList = _roleService.GetRoleList().Select(r => new SelectListItem() { Value = r.Id.ToString(), Text = r.RoleName }).ToList();
            return View(vm);
        }
        [Authorize(Roles="Admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var user = _userService.GetUserById(id);
            if(user!= null)
            {
                _userService.DeleteUser(user);
                return RedirectToAction("UserList", "User");
            }
            return NotFound();
        }
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
