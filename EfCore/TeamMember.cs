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

        /// <summary>
        /// Added By (Manager)
        /// </summary>
        [Required]
        public Guid AddedById { get; set; }

        /// <summary>
        /// Date added to the team
        /// </summary>
        public DateTime AddedOn { get; set; } = DateTime.UtcNow;

        // Navigation Properties

        /// <summary>
        /// Team
        /// </summary>
        [ForeignKey("TeamId")]
        public Team Team { get; set; }

        /// <summary>
        /// User (Team Member)
        /// </summary>
        [ForeignKey("UserId")]
        public User User { get; set; }

        /// <summary>
        /// Added By (Manager)
        /// </summary>
        [ForeignKey("AddedById")]
        public User AddedBy { get; set; }
    }
}
