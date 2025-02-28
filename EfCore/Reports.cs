using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfCore
{
    public class Reports
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid EmployeeId { get; set; } // FK to Users table (who submitted)

        [Required]
        public Guid ManagerId { get; set; } // FK to Users table (who receives)

        [Required]
        public string Content { get; set; } // Report details

        [Required]
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("EmployeeId")]
        public Users Employee { get; set; }

        [ForeignKey("ManagerId")]
        public Users Manager { get; set; }

    }
}
