using DomainModel.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;
        private readonly CustomIDataProtection _protect;
        public UserRepository(DatabaseContext context, CustomIDataProtection protector)
        {
            _context = context;
            _protect = protector;
        }
        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public void Edit(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public User GetUserById(int id)
        {
            return _context.Users.Include(u => u.Role).Include(u => u.userTickets).FirstOrDefault(u=>u.Id == id);
        }

        public User GetUserByUserName(string username)
        {
            return _context.Users.Include(u => u.Role).Include(u => u.userTickets).FirstOrDefault(u => u.UserName == username);
        }

        public IEnumerable<User> UserList()
        {
            return _context.Users.Include(u => u.Role).Include(u=>u.userTickets).ToList().Select(e=> { e.EncryptedId = _protect.Encode(e.Id.ToString()); return e; });
        }
    }
}
