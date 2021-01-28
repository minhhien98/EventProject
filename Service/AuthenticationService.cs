using DomainModel.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Repository;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using IAuthenticationService = Service.Interface.IAuthenticationService;

namespace Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContext;
        public AuthenticationService(IUserRepository userRepository, IHttpContextAccessor httpContext)
        {
            _userRepository = userRepository;
            _httpContext = httpContext;
        }
        public User Login(string username, string password, bool rememberme)
        {
            var user = _userRepository.UserList().FirstOrDefault(u => u.UserName == username && u.Password == password);
            if (user != null)
            {
                if (!user.EmailConfirmed)
                {
                    return user;
                }
                var claims = userClaim(user);
                var authenprops = new AuthenticationProperties();
                if (rememberme)
                {
                    authenprops.IsPersistent = true;
                    authenprops.ExpiresUtc = DateTime.UtcNow.AddYears(1);
                }
                var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
                //Extension: aspnetcore.authentication.cookies
                _httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authenprops);
                return user;
            }
            else
            {
                return null;
            }
        }

        public void Logout()
        {
            _httpContext.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        private IEnumerable<Claim> userClaim(User user)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name , user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,user.Role.RoleName),
                new Claim(ClaimTypes.NameIdentifier,user.FirstName)
            };

            return claims;
        }
    }
}
