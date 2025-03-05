using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using EfCore;

namespace DTOs.PRReviews
{
    public class PRReviewRequest
    {
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

        [Required]
        [MaxLength(2000)]
        /// <summary>
        /// Review Comments
        /// </summary>
        public string? ReviewComments { get; set; }

        public Review ToReview()
        {
            return new Review
            {
                PullRequestId = this.PullRequestId,
                ReviewerId = this.ReviewerId,
                ReviewStatus = this.ReviewStatus,
                ReviewComments = this.ReviewComments
            };
        }
    }
}
