using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCore
{
    public class Tasks
    {
        /// <summary>
        /// Task Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }

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
        /// Task assigned by Id (Manager Id
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
        public string TaskStatus { get; set; }

        /// <summary>
        /// Task deadline
        /// </summary>
        [Required]
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Task Created At
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties

        [ForeignKey("AssignedBy")]
        public Users Assigner { get; set; }

        [ForeignKey("AssignedTo")]
        public Users Assignee { get; set; }
        public ICollection<TaskComments> Comments { get; set; } = new List<TaskComments>();
        public ICollection<TaskAttachments> Attachments { get; set; } = new List<TaskAttachments>();
        public ICollection<PullRequests> PullRequests { get; set; } = new List<PullRequests>();


    }
}
