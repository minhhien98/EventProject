using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface IAuthenticationService
    {
        bool Login(string username, string password);
        void Logout();
    }
}
