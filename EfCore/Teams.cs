using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCore
{
    public class Teams
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
        public string? TeamName { get; set; }

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
        public Users Manager { get; set; }

        /// <summary>
        /// Team Members
        /// </summary>
        public ICollection<TeamMembers> TeamMembers { get; set; } = new List<TeamMembers>();

        /// <summary>
        /// Team Meetings
        /// </summary>
        public ICollection<ScrumMeetings> ScrumMeetings { get; set; } = new List<ScrumMeetings>();
    }

}
