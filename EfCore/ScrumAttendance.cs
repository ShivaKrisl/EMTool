using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public bool IsPresent { get; set; } = false;

        /// <summary>
        /// Scrum meetings notes (remarks)
        /// </summary>
        public string? Notes { get; set; }

        // Navigation Properties

        [ForeignKey("MeetingId")]
        public ScrumMeetings scrumMeeting { get; set; }

        [ForeignKey("UserId")]
        public Users user { get; set; }
    }
}
