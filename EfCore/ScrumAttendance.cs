using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCore
{
    public class ScrumAttendance
    {
        /// <summary>
        /// Id of Scrum Attendance
        /// </summary>
        [Key]
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

        // Navigation Properties

        [ForeignKey("MeetingId")]
        public ScrumMeeting ScrumMeeting { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
