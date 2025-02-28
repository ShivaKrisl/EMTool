using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCore
{
    public class TaskComments
    {
        /// <summary>
        /// Comment Id
        /// </summary>
        [Key]
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
        public string? Comment { get; set; }

        /// <summary>
        /// Commented On
        /// </summary>
        [Required]
        public DateTime CommentedOn { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("TaskId")]
        public Tasks Task { get; set; }

        [ForeignKey("UserId")]
        public Users User { get; set; }
    }
}
