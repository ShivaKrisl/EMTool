using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs.Tasks;
using EfCore;
using DTOs.Users;

namespace DTOs.WorkAttachments
{
    public class WorkAttachmentResponse
    {
        /// <summary>
        /// Task Attachment Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Task Id to which file is attached
        /// </summary>
        [Required]
        public Guid TaskId { get; set; }

        /// <summary>
        /// User Id who uploaded the file
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// File Name
        /// </summary>
        [Required]
        public string FileName { get; set; }

        /// <summary>
        /// File Path
        /// </summary>
        [Required]
        public string FilePath { get; set; }

        /// <summary>
        /// File Type
        /// </summary>
        [Required]
        public string FileType { get; set; }

        /// <summary>
        /// Attachment Created At Timestamp
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Task to which file is attached
        /// </summary>
        [Required]
        public WorkResponse Task { get; set; }

        /// <summary>
        /// User who uploaded the file
        /// </summary>
        [Required]
        public UserResponse User { get; set; }

    }

    public static class WorkAttachmentResponseExtensions
    {
        public static WorkAttachmentResponse ToWorkAttachmentResponse(this WorkAttachment workAttachment, WorkResponse workResponse, UserResponse userResponse)
        {
            return new WorkAttachmentResponse
            {
                Id = workAttachment.Id,
                TaskId = workAttachment.TaskId,
                UserId = workAttachment.UserId,
                FileName = workAttachment.FileName,
                FilePath = workAttachment.FilePath,
                FileType = workAttachment.FileType,
                CreatedAt = workAttachment.CreatedAt,
                Task = workResponse,
                User = userResponse
            };
        }
    }
}
