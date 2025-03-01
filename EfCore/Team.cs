using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCore
{
    public class Team
    {
        /// <summary>
        ///  Team Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Team Name
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string TeamName { get; set; }  // Removed nullable (?)

        /// <summary>
        /// Team Manager Id
        /// </summary>
        [Required]
        public Guid ManagerId { get; set; }

        // Navigation Property

        /// <summary>
        /// Team Manager
        /// </summary>
        [ForeignKey("ManagerId")]
        public User Manager { get; set; }

        /// <summary>
        /// Team Members
        /// </summary>
        public ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();

        /// <summary>
        /// Team Meetings
        /// </summary>
        public ICollection<ScrumMeeting> ScrumMeetings { get; set; } = new List<ScrumMeeting>();
    }
}
