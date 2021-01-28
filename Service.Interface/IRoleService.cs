using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface IRoleService
    {
        void AddRole(Role role);
        void EditRole(Role role);
        void DeleteRole(Role role);
        IEnumerable<Role> GetRoleList();
        Role GetRoleById(int id);
    }
}
