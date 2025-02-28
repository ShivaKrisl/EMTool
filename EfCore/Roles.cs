using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCore
{
    public class Roles
    {
        /// <summary>
        /// Role Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Role Name (Manager, Employee)
        /// </summary>
        [Required]
        public string Role { get; set; }

        // navigation property

        /// <summary>
        /// Users Table that describe role of employee
        /// </summary>
        public ICollection<Users> Users { get; set; } = new List<Users>();


    }
}
