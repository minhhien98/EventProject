using DomainModel.Entities;
using Repository;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        public UserService()
        {
            userRepository = new UserRepository();
        }
        public void AddUser(User user)
        {
            userRepository.Add(user);
        }

        public void DeleteUser(User user)
        {
            userRepository.Delete(user);
        }

        public void EditUser(User user)
        {
            userRepository.Edit(user);
        }

        public User GetUserById(int id)
        {
            return userRepository.GetUserById(id);
        }

        public User GetUserByUserName(string username)
        {
            return userRepository.GetUserByUserName(username);
        }

        public IEnumerable<User> GetUserList()
        {
            return userRepository.UserList();
        }
    }
}
