using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs.Roles;

namespace Service_Contracts
{
    public interface IRoleService
    {

        /// <summary>
        /// Create a new Role
        /// </summary>
        /// <param name="roleRequest"></param>
        /// <returns></returns>
        public Task<RoleResponse> CreateRole(RoleRequest? roleRequest);

        /// <summary>
        /// Get a User Role by Role Id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public Task<RoleResponse>? GetRoleById(Guid roleId);

        /// <summary>
        /// Get a User Role by Role Name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public Task<RoleResponse>? GetRoleByName(string roleName);



    }
}
