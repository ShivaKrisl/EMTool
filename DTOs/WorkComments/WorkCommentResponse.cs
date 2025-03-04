using EfCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs.Users;
using DTOs.Tasks;

namespace DTOs.WorkComments
{
    public class WorkCommentResponse
    {
        /// <summary>
        /// Comment Id
        /// </summary>
        [Required]
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
        public string Comment { get; set; }

        /// <summary>
        /// Commented On
        /// </summary>
        [Required]
        public DateTime CommentedOn { get; set; }

        // Navigation Properties
        [Required]
        public WorkResponse Task { get; set; } 

        [Required]
        public UserResponse User { get; set; }
    }

    public static class WorkCommentResponseExtensions
    {
        public static WorkCommentResponse ToWorkCommentResponse(this WorkComment workComment, WorkResponse workResponse, UserResponse userResponse)
        {
            return new WorkCommentResponse()
            {
                Id = workComment.Id,
                UserId = workComment.UserId,
                TaskId = workComment.TaskId,
                Comment = workComment.Comment,
                CommentedOn = workComment.CommentedOn,
                Task = workResponse,
                User = userResponse
            };
        }
    }
}
