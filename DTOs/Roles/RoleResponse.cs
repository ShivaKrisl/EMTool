﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfCore;

namespace DTOs.Roles
{
    public class RoleResponse
    {
        /// <summary>
        /// Role Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Role Name (e.g., Manager, Employee)
        /// </summary>
        [Required]
        public string Name { get; set; }
    }

    public static class RoleResponseExtensions
    {
        public static RoleResponse ToRoleResponse(this Role role)
        {
            return new RoleResponse()
            {
                Id = role.Id,
                Name = role.Name,
            };
        }

        public static Role ToRole(this RoleResponse roleResponse)
        {
            return new Role()
            {
                Id = roleResponse.Id,
                Name = roleResponse.Name,
            };
        }
    }
}
