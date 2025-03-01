using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCore
{
    public class WorkAttachment
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
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("TaskId")]
        public Work Task { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
