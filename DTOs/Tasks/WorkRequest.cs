using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfCore;

namespace DTOs.Tasks
{
    public class WorkRequest
    {
        /// <summary>
        /// Task Title
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        /// <summary>
        /// Task Description
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Task assigned by Id (Manager Id)
        /// </summary>
        [Required]
        public Guid AssignedBy { get; set; }

        /// <summary>
        /// Task assigned to Id (Employee Id)
        /// </summary>
        [Required]
        public Guid AssignedTo { get; set; }

        /// <summary>
        /// Team Id
        /// </summary>
        [Required]
        public Guid TeamId { get; set; }

        /// <summary>
        /// Task Status
        /// </summary>
        [Required]
        public string Status { get; set; }

        /// <summary>
        /// Task deadline
        /// </summary>
        [Required]
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Task Created At
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Work ToWork()
        {
            return new Work
            {
                Title = Title,
                Description = Description,
                AssignedBy = AssignedBy,
                AssignedTo = AssignedTo,
                TeamId = TeamId,
                Status = Status,
                Deadline = Deadline,
                CreatedAt = CreatedAt
            };
        }
    }
}
