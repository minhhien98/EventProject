using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Interface
{
    public interface IRoleRepository
    {
        void Add(Role role);
        void Edit(Role role);
        void Delete(Role role);
        IEnumerable<Role> RoleList();
        Role GetRoleById(int id);
    }
}
