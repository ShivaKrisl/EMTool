using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCore
{
    public class TeamMember
    {
        /// <summary>
        /// Team Members Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Team Id
        /// </summary>
        [Required]
        public Guid TeamId { get; set; }

        /// <summary>
        /// User Id
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        // Navigation Properties

        /// <summary>
        /// Team
        /// </summary>
        [ForeignKey("TeamId")]
        public Team Team { get; set; }

        /// <summary>
        /// User
        /// </summary>
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
