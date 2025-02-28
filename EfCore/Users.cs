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
    public class Users
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
        public Roles Role { get; set; }

        /// <summary>
        /// Employee Created At Date
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        /// <summary>
        /// Employee Updated At Date stamp
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties

        public ICollection<Tasks> AssignedTasks { get; set; } = new List<Tasks>();
        public ICollection<TeamMembers> TeamMembers { get; set; } = new List<TeamMembers>();
        public ICollection<PullRequests> PullRequests { get; set; } = new List<PullRequests>();
        public ICollection<TaskComments> Comments { get; set; } = new List<TaskComments>();
        public ICollection<Reviews> ReviewsGiven { get; set; } = new List<Reviews>();
        public ICollection<Reviews> ReviewsReceived { get; set; } = new List<Reviews>();
        public ICollection<Notifications> Notifications { get; set; } = new List<Notifications>();
        public ICollection<ScrumAttendance> ScrumAttendances { get; set; } = new List<ScrumAttendance>();
        public ICollection<Reports> SubmittedReports { get; set; } = new List<Reports>();
        public ICollection<Reports> ReceivedReports { get; set; } = new List<Reports>();

        public ICollection<Teams> ManagedTeams { get; set; } = new List<Teams>();
    }
}
