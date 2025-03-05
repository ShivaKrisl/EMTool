using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCore
{
    public class PullRequest
    {
        /// <summary>
        /// Pull Request Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Task Id to which the pull request is linked
        /// </summary>
        [Required]
        public Guid TaskId { get; set; }

        /// <summary>
        /// User who created the pull request
        /// </summary>
        [Required]
        public Guid CreatedById { get; set; }

        /// <summary>
        /// Pull Request URL
        /// </summary>
        [Required]
        [Url]
        public string PRLink { get; set; }

        /// <summary>
        /// Pull Request Description
        /// </summary>
        [MaxLength(2000)] // Set a reasonable max length
        public string? PRDescription { get; set; }

        /// <summary>
        /// Path for PR explanation attachment
        /// </summary>
        [Required]
        public string AttachmentPath { get; set; }

        /// <summary>
        /// Pull Request Status
        /// </summary>
        [Required]
        [MaxLength(50)] // Limit status length (e.g., "Pending", "Approved", "Rejected")
        public string PRStatus { get; set; }

        /// <summary>
        /// Flag to indicate if PR is ready for approvals
        /// </summary>  
        [Required]
        public bool IsReadyForApproval { get; set; } = false;

        /// <summary>
        /// Timestamp when PR was created
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties

        [ForeignKey("TaskId")]
        public Work Task { get; set; }

        [ForeignKey("CreatedById")]
        public User CreatedBy { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
