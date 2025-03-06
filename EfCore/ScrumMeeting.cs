using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCore
{
    public class ScrumMeeting
    {
        /// <summary>
        /// Id of Scrum Meeting
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Id of the Team conducting the Scrum Meeting
        /// </summary>
        [Required]
        public Guid TeamId { get; set; }

        /// <summary>
        /// Date of Scrum Meeting
        /// </summary>
        [Required]
        public DateTime ScheduledDate { get; set; }

        /// <summary>
        /// Agenda of Scrum Meeting
        /// </summary>
        [Required]
        public string Agenda { get; set; }

        /// <summary>
        /// Link of Scrum Meeting
        /// </summary>
        [Required]
        public string MeetingLink { get; set; }

        /// <summary>
        /// User Id of the creator of the Scrum Meeting
        /// </summary>
        [Required]
        public Guid CreatedBy { get; set; }


        /// <summary>
        /// Role of the creator of the Scrum Meeting
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string CreatedByRole { get; set; }

        /// <summary>
        /// List of User Ids invited to the Scrum Meeting
        /// </summary>
        [Required]
        public ICollection<Guid> InvitedUsersIds { get; set; } = new List<Guid>();

        // Navigation Properties

        [ForeignKey("TeamId")]
        public Team Team { get; set; }

        [ForeignKey("CreatedBy")]
        public User CreatedByUser { get; set; }

        public ICollection<ScrumAttendance> ScrumAttendances { get; set; } = new List<ScrumAttendance>();
    }
}
