using EfCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs.Users;
using DTOs.PullRequests;

namespace DTOs.PRReviews
{
    public class PRReviewResponse
    {
        /// Review Id
        /// </summary>
        [Required]
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

        [Required]
        [MaxLength(2000)]
        /// <summary>
        /// Review Comments
        /// </summary>
        public string? ReviewComments { get; set; }

        [Required]
        /// <summary>
        /// Review Date
        /// </summary>
        public DateTime ReviewDate { get; set; }

        [Required]
        public PREntityResponse PullRequest { get; set; }

        [Required]
        public UserResponse Reviewer { get; set; }
    }

    public static class ReviewResponseExtensions
    {
        public static PRReviewResponse ToPRReviewResponse(this Review review)
        {
            return new PRReviewResponse
            {
                Id = review.Id,
                PullRequestId = review.PullRequestId,
                ReviewerId = review.ReviewerId,
                ReviewStatus = review.ReviewStatus,
                ReviewComments = review.ReviewComments,
                ReviewDate = review.ReviewDate,
                PullRequest = review.PullRequest.ToPREntityResponse(),
                Reviewer = review.Reviewer.ToUserResponse()
            };
        }
    }

}
