using EfCore;
using System.ComponentModel.DataAnnotations;
using DTOs.Users;
using DTOs.Teams;
using DTOs.Roles;

namespace DTOs.ScrumMeetings
{
    public class ScrumMeetingResponse
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

        // Navigation Properties

        [Required]
        public TeamResponse Team { get; set; }

        [Required]
        public UserResponse CreatedByUser { get; set; }
    }

    public static class ScrumMeetingResponseExtensions
    {
        public static ScrumMeetingResponse ToScrumMeetingResponse(this ScrumMeeting scrumMeeting)
        {
            return new ScrumMeetingResponse()
            {
                Id = scrumMeeting.Id,
                TeamId = scrumMeeting.TeamId,
                ScheduledDate = scrumMeeting.ScheduledDate,
                Agenda = scrumMeeting.Agenda,
                MeetingLink = scrumMeeting.MeetingLink,
                Team = scrumMeeting.Team.ToTeamResponse(),
                CreatedBy = scrumMeeting.CreatedBy,
                CreatedByRole = scrumMeeting.CreatedByRole,
                InvitedUserIds = scrumMeeting.InvitedUsersIds.ToList(),
                CreatedByUser = scrumMeeting.CreatedByUser.ToUserResponse()
            };
        }

        public static ScrumMeeting ToScrumMeeting(this ScrumMeetingResponse scrumMeetingResponse, UserResponse managerUserResponse, RoleResponse roleResponse, RoleResponse managerRole )
        {
            return new ScrumMeeting()
            {
                Id = scrumMeetingResponse.Id,
                TeamId = scrumMeetingResponse.TeamId,
                ScheduledDate = scrumMeetingResponse.ScheduledDate,
                Agenda = scrumMeetingResponse.Agenda,
                MeetingLink = scrumMeetingResponse.MeetingLink,
                CreatedBy = scrumMeetingResponse.CreatedBy,
                CreatedByRole = scrumMeetingResponse.CreatedByRole,
                InvitedUsersIds = scrumMeetingResponse.InvitedUserIds,
                CreatedByUser = scrumMeetingResponse.CreatedByUser.ToUser(roleResponse.ToRole()),
                Team = scrumMeetingResponse.Team.ToTeam(managerUserResponse.ToUser(managerRole.ToRole()))
            };
        }
    }

}
