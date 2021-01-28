using DomainModel.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext context;
        public UserRepository()
        {
            context = new DatabaseContext();
        }
        public void Add(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
        }

        public void Delete(User user)
        {
            context.Users.Remove(user);
            context.SaveChanges();
        }

        public void Edit(User user)
        {
            context.Entry(user).State = EntityState.Modified;
            context.SaveChanges();
        }

        public User GetUserById(int id)
        {
            return context.Users.Include(u => u.Role).FirstOrDefault(u=>u.Id == id);
        }

        public User GetUserByUserName(string username)
        {
            return context.Users.Include(u => u.Role).FirstOrDefault(u => u.UserName == username);
        }

        public IEnumerable<User> UserList()
        {
            return context.Users.ToList();
        }
    }
}
