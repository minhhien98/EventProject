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
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public void AddUser(User user)
        {
            _userRepository.Add(user);
        }

        public void DeleteUser(User user)
        {
            _userRepository.Delete(user);
        }

        public void EditUser(User user)
        {
            _userRepository.Edit(user);
        }

        public User GetUserById(int id)
        {
            return _userRepository.GetUserById(id);
        }

        public User GetUserByUserName(string username)
        {
            return _userRepository.GetUserByUserName(username);
        }

        public IEnumerable<User> GetUserList()
        {
            return _userRepository.UserList();
        }
    }
}
