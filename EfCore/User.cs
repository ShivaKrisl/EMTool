using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace EfCore
{
    public class User
    {
        /// <summary>
        /// Employee Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Employee First Name
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string? FirstName { get; set; }

        /// <summary>
        /// Employee Last Name
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string? LastName { get; set; }

        /// <summary>
        /// Employee Email
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string? Email { get; set; }

        /// <summary>
        /// Employee Username
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string? Username { get; set; }

        /// <summary>
        /// Employee Password Hash
        /// </summary>
        [Required]
        public string? PasswordHash { get; set; }  // Storing password as a hash for security

        /// <summary>
        /// Employee Role Id
        /// </summary>
        [Required]
        public Guid RoleId { get; set; }

        /// <summary>
        /// Employee Role
        /// </summary>
        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        /// <summary>
        /// Employee Created At Date
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        /// <summary>
        /// Employee Updated At Date stamp
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties

        public ICollection<Work> AssignedTasks { get; set; } = new List<Work>();

        public ICollection<WorkAttachment> workAttachments { get; set; } = new List<WorkAttachment>();

        public ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();

        public ICollection<TeamMember> AddedTeamMembers { get; set; } = new List<TeamMember>();

        public ICollection<PullRequest> PullRequests { get; set; } = new List<PullRequest>();
        public ICollection<WorkComment> Comments { get; set; } = new List<WorkComment>();
        public ICollection<Review> ReviewsGiven { get; set; } = new List<Review>();
        public ICollection<Review> ReviewsReceived { get; set; } = new List<Review>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<ScrumAttendance> ScrumAttendances { get; set; } = new List<ScrumAttendance>();

        public ICollection<Team> ManagedTeams { get; set; } = new List<Team>();
    }
}
