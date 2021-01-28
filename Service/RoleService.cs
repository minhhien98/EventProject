using DomainModel.Entities;
using Repository;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;

namespace Service
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository roleRepository;
        public RoleService()
        {
            roleRepository = new RoleRepository();
        }
        public void AddRole(Role role)
        {
            roleRepository.Add(role);
        }

        public void DeleteRole(Role role)
        {
            roleRepository.Delete(role);
        }

        public void EditRole(Role role)
        {
            roleRepository.Edit(role);
        }

        public Role GetRoleById(int id)
        {
            return roleRepository.GetRoleById(id);
        }

        public IEnumerable<Role> GetRoleList()
        {
            return roleRepository.RoleList();
        }
    }
}
