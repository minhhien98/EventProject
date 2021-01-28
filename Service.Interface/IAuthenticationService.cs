using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface IAuthenticationService
    {
        User Login(string username, string password,bool rememberme);
        void Logout();
    }
}
