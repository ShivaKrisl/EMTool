using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCore
{
    public class Review
    {
        /// <summary>
        /// Review Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Pull Request Id
        /// </summary>
        [Required]
        public Guid PullRequestId { get; set; }

        /// <summary>
        /// Reviewer Id (Employee Id) who reviewed the pull request
        /// </summary>
        [Required]
        public Guid ReviewerId { get; set; }

        /// <summary>
        /// Review Status (Approved/Rejected)
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string ReviewStatus { get; set; }

        /// <summary>
        /// Review Comments
        /// </summary>
        public string? ReviewComments { get; set; }

        /// <summary>
        /// Review Date
        /// </summary>
        public DateTime ReviewDate { get; set; } = DateTime.UtcNow;

        // Navigation Properties

        [ForeignKey("PullRequestId")]
        public PullRequest PullRequest { get; set; }

        [ForeignKey("ReviewerId")]
        public User Reviewer { get; set; }
    }
}
