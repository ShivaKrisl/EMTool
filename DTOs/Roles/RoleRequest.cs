using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using EfCore;

namespace DTOs.Roles
{
    public class RoleRequest
    {
        /// <summary>
        /// Role Name
        /// </summary>
        [Required]
        public string Name { get; set; }

        public Role ToRole()
        {
            return new Role()
            {
                Name = Name
            };
        }
    }
}
