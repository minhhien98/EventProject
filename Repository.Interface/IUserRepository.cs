using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Interface
{
    public interface IUserRepository
    {
        void Add(User user);
        void Edit(User user);
        void Delete(User user);
        IEnumerable<User> UserList();
        User GetUserById(int id);
        User GetUserByUserName(string username);

    }
}
