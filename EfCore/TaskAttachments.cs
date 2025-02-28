using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCore
{
    public class TaskAttachments
    {
        /// <summary>
        /// Task Attachement Id
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
        public string? FileType { get; set; }

        /// <summary>
        /// File Size
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties

        [ForeignKey("TaskId")]
        public Tasks tasks { get; set; }

        [ForeignKey("UserId")]
        public Users users { get; set; }
    }
}
