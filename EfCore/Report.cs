using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCore
{
    public class Report
    {
        /// <summary>
        /// Unique Report Identifier
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Employee (User) who submitted the report
        /// </summary>
        [Required]
        public Guid SubmittedById { get; set; }

        /// <summary>
        /// Manager (User) who reviews the report
        /// </summary>
        [Required]
        public Guid ReviewedById { get; set; }

        /// <summary>
        /// Report details
        /// </summary>
        [MaxLength(2000)] // Adjust as needed
        public string? Content { get; set; }

        /// <summary>
        /// Report submission timestamp
        /// </summary>
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("SubmittedById")]
        public User SubmittedBy { get; set; }

        [ForeignKey("ReviewedById")]
        public User ReviewedBy { get; set; }
    }
}
