using EfCore;
using System.ComponentModel.DataAnnotations;
using DTOs.ScrumMeetings;
using DTOs.Users;

namespace DTOs.ScrumAttendances
{
    public class ScrumAttendanceResponse
    {
        /// <summary>
        /// Id of Scrum Attendance
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Scrum Meeting Id
        /// </summary>
        [Required]
        public Guid MeetingId { get; set; }

        /// <summary>
        /// User Id of Employee
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// Indicates if the user was present in the meeting
        /// </summary>
        public bool IsPresent { get; set; } = false;

        /// <summary>
        /// Scrum meeting notes (remarks)
        /// </summary>
        public string? Notes { get; set; }

        [Required]
        public ScrumMeetingResponse ScrumMeeting { get; set; }

        [Required]
        public UserResponse User { get; set; }
    }

    public static class ScrumAttendanceResponseExtensions
    {
        public static ScrumAttendanceResponse ToScrumAttendanceResponse(this ScrumAttendance scrumAttendance)
        {
            return new ScrumAttendanceResponse()
            {
                Id = scrumAttendance.Id,
                MeetingId = scrumAttendance.MeetingId,
                UserId = scrumAttendance.UserId,
                IsPresent = scrumAttendance.IsPresent,
                Notes = scrumAttendance.Notes,
                ScrumMeeting = scrumAttendance.ScrumMeeting.ToScrumMeetingResponse(),
                User = scrumAttendance.User.ToUserResponse()
            };
        }
    }
}
