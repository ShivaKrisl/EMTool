using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCore
{
    public class PullRequests
    {
        /// <summary>
        /// Pull Request Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Task Id to which pull request is created
        /// </summary>
        [Required]
        public Guid TaskId { get; set; }

        /// <summary>
        /// User Id who created the pull request
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// Pull Request Url
        /// </summary>
        [Required]
        [Url]
        public string PRLink { get; set; }

        /// <summary>
        /// Pull Request Description
        /// </summary>
        [Required]
        public string? PRDescription { get; set; }

        /// <summary>
        /// Pull Request explanation Attachement Path
        /// </summary>
        [Required]
        public string AttachementPath { get; set; }

        /// <summary>
        /// Pull Request Status
        /// </summary>
        [Required]
        public string PRStatus { get; set; }

        /// <summary>
        /// Pull Request Created At
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties

        [ForeignKey("TaskId")]
        public Tasks task { get; set; }

        [ForeignKey("UserId")]
        public Users user { get; set; }

        public ICollection<Reviews> reviews { get; set; }
    }
}
