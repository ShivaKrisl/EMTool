using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EfCore
{
    public class Role
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

        /// <summary>
        /// Users table that describes the role of an employee
        /// </summary>
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
