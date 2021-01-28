using DomainModel.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DatabaseContext context;
        public RoleRepository()
        {
            context = new DatabaseContext();
        }
        public void Add(Role role)
        {
            context.Roles.Add(role);
            context.SaveChanges();
        }

        public void Delete(Role role)
        {
            context.Roles.Remove(role);
            context.SaveChanges();
        }

        public void Edit(Role role)
        {
            context.Entry(role).State = EntityState.Modified;
            context.SaveChanges();
        }

        public Role GetRoleById(int id)
        {
            return context.Roles.Find(id);
        }

        public IEnumerable<Role> RoleList()
        {
            return context.Roles.ToList();
        }
    }
}
