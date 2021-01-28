using DomainModel.Entities;
using System;
using System.Collections.Generic;

namespace Service.Interface
{
    public interface IUserService
    {
        void AddUser(User user);
        void EditUser(User user);
        void DeleteUser(User user);
        IEnumerable<User> GetUserList();
        User GetUserById(int id);
        User GetUserByUserName(string username);
    }
}
