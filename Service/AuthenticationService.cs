using Repository;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository userRepository;
        public AuthenticationService()
        {
            userRepository = new UserRepository();
        }
        public bool Login(string username, string password)
        {
            var user = userRepository.UserList().FirstOrDefault(u => u.UserName == username && u.Password == password);
            if(user != null)
            {
                return true;
            }
            return false;
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }
    }
}
