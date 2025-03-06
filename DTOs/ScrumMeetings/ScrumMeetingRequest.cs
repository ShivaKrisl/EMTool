using EfCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ScrumMeetings
{
    public class ScrumMeetingRequest
    {
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
        [Url]
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
        public List<Guid> InvitedUserIds { get; set; }

        public ScrumMeeting ToScrumMeeting()
        {
            return new ScrumMeeting
            {
                TeamId = this.TeamId,
                ScheduledDate = this.ScheduledDate,
                Agenda = this.Agenda,
                MeetingLink = this.MeetingLink,
                CreatedBy = this.CreatedBy,
                CreatedByRole = this.CreatedByRole,
                InvitedUsersIds = this.InvitedUserIds
            };
        }
    }
}
