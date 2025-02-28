using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCore
{
    public class Reviews
    {
        /// <summary>
        /// Review Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Pull Request Id
        /// </summary>
        [Required]
        public Guid PRId { get; set; }

        /// <summary>
        /// User Reviewer Id (Employee Id) who review the pull request
        /// </summary>
        [Required]
        public Guid UserReviewerId { get; set; }

        /// <summary>
        /// Review Status (Approved/Rejected)
        /// </summary>
        [Required]
        public string ReviewStatus { get; set; }

        /// <summary>
        /// Review Comments
        /// </summary>
        [Required]
        public string ReviewComments { get; set; }

        /// <summary>
        /// Review Date
        /// </summary>
        public DateTime ReviewDate { get; set; } = DateTime.Now;

        // Navigation Properties

        [ForeignKey("PRId")]    
        public PullRequests pullRequests { get; set; }

        [ForeignKey("UserReviewerId")]
        public Users users { get; set; }
    }
}
