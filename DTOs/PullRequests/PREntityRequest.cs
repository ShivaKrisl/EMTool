using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfCore;

namespace DTOs.PullRequests
{
    public class PREntityRequest
    {
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
        public string? PRLink { get; set; }

        /// <summary>
        /// Pull Request Description
        /// </summary>
        [MaxLength(2000)] // Set a reasonable max length
        public string? PRDescription { get; set; }

        /// <summary>
        /// Path for PR explanation attachment
        /// </summary>
        [Required]
        public string? AttachmentPath { get; set; }

        /// <summary>
        /// Pull Request Status
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string? PRStatus { get; set; }

        public PullRequest ToPullRequest()
        {
            return new PullRequest()
            {
                TaskId = TaskId,
                CreatedById = CreatedById,
                PRLink = PRLink!,
                PRDescription = PRDescription,
                AttachmentPath = AttachmentPath!,
                PRStatus = PRStatus!,
            };
        }

    }
}
