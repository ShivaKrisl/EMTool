using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCore
{
    public class Work
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

        // Navigation Properties
        [ForeignKey("AssignedBy")]
        public User Assigner { get; set; }

        [ForeignKey("AssignedTo")]
        public User Assignee { get; set; }

        public ICollection<WorkComment> Comments { get; set; } = new List<WorkComment>();
        public ICollection<WorkAttachment> Attachments { get; set; } = new List<WorkAttachment>();
        public ICollection<PullRequest> PullRequests { get; set; } = new List<PullRequest>();
    }

    
}
