using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCore
{
    public class WorkComment
    {
        /// <summary>
        /// Comment Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Task Id
        /// </summary>
        [Required]
        public Guid TaskId { get; set; }

        /// <summary>
        /// User Id
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// Comment
        /// </summary>
        [Required]
        public string Comment { get; set; }  // Fixed nullable issue

        /// <summary>
        /// Commented On
        /// </summary>
        [Required]
        public DateTime CommentedOn { get; set; } = DateTime.UtcNow;  // Use UTC time

        // Navigation Properties
        [ForeignKey("TaskId")]
        public Work Task { get; set; }  // Renamed to avoid conflict

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
