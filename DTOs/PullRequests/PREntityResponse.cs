using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using EfCore;

namespace DTOs.PullRequests
{
    public class PREntityResponse
    {
        /// <summary>
        /// Pull Request Id
        /// </summary>
        [Required]
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
        public bool IsReadyForApproval { get; set; }

        /// <summary>
        /// Timestamp when PR was created
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; }

        // Navigation Properties

        [Required]
        public Work Task { get; set; }

        [Required]
        public User CreatedBy { get; set; }
    }

    public static class PRResponseExtensions
    {
        public static PREntityResponse ToPREntityResponse(this PullRequest pullRequest)
        {
            return new PREntityResponse()
            {
                Id = pullRequest.Id,
                TaskId = pullRequest.TaskId,
                CreatedById = pullRequest.CreatedById,
                PRLink = pullRequest.PRLink,
                PRDescription = pullRequest.PRDescription,
                AttachmentPath = pullRequest.AttachmentPath,
                PRStatus = pullRequest.PRStatus,
                CreatedAt = pullRequest.CreatedAt,
                Task = pullRequest.Task,
                CreatedBy = pullRequest.CreatedBy,
                IsReadyForApproval = pullRequest.IsReadyForApproval
            };
        }

        public static PullRequest ToPullRequest(this PREntityResponse pREntityResponse)
        {
            return new PullRequest()
            {
                Id = pREntityResponse.Id,
                TaskId = pREntityResponse.TaskId,
                CreatedById = pREntityResponse.CreatedById,
                PRLink = pREntityResponse.PRLink,
                PRDescription = pREntityResponse.PRDescription,
                AttachmentPath = pREntityResponse.AttachmentPath,
                PRStatus = pREntityResponse.PRStatus,
                CreatedAt = pREntityResponse.CreatedAt,
                Task = pREntityResponse.Task,
                CreatedBy = pREntityResponse.CreatedBy,
                IsReadyForApproval = pREntityResponse.IsReadyForApproval
            };
        }
    }
}
