using DTOs.Roles;
using Service_Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfCore;

namespace Services
{
    public class RoleService : IRoleService
    {

        private readonly List<Role> _mockRoles;

        public RoleService()
        {
            _mockRoles = new List<Role>();
        }

        /// <summary>
        /// Create a new Role
        /// </summary>
        /// <param name="roleRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<RoleResponse> CreateRole(RoleRequest roleRequest)
        {
            if (roleRequest == null)
            {
                throw new ArgumentNullException(nameof(roleRequest));
            }

            bool isModelValid = ValidationHelper.IsStateValid(roleRequest);
            if (!isModelValid)
            {
                throw new ArgumentException("RoleRequest is not valid");
            }

            bool roleExists = _mockRoles.Any(r => r.Name == roleRequest.Name);

            if (roleExists)
            {
                throw new ArgumentException("Role already exists");
            }

            Role role = roleRequest.ToRole();
            role.Id = Guid.NewGuid();
            _mockRoles.Add(role);

            return Task.FromResult(role.ToRoleResponse());

        }

        /// <summary>
        /// Get a User Role by Role Id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<RoleResponse>? GetRoleById(Guid roleId)
        {
            if(roleId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(roleId));
            }

            Role? role = _mockRoles.FirstOrDefault(r => r.Id == roleId);
            if(role == null)
            {
                return null;
            }
            return Task.FromResult(role.ToRoleResponse());

        }

        /// <summary>
        /// Get a User Role by Role Name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<RoleResponse>? GetRoleByName(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentNullException(nameof(roleName));
            }

            Role? role = _mockRoles.FirstOrDefault(r => r.Name.Equals(roleName));
            if (role == null)
            {
                return null;
            }
            return Task.FromResult<RoleResponse>(role.ToRoleResponse());
        }
    }
}
