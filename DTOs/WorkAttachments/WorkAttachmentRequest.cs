using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using EfCore;

namespace DTOs.WorkAttachments
{
    public class WorkAttachmentRequest
    {
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

        public WorkAttachment ToWorkAttachment()
        {
            return new WorkAttachment
            {
                TaskId = this.TaskId,
                UserId = this.UserId,
                FileName = this.FileName,
                FilePath = this.FilePath,
                FileType = this.FileType,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
