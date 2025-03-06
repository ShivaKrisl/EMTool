using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Formats.Tar;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EfCore;

namespace DTOs.ScrumAttendances
{
    public class ScrumAttendanceRequest
    {
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
        public bool IsPresent { get; set; }

        /// <summary>
        /// Scrum meeting notes (remarks)
        /// </summary>
        public string? Notes { get; set; }

        public ScrumAttendance ToScrumAttendance()
        {
            return new ScrumAttendance()
            {
                MeetingId = this.MeetingId,
                UserId = this.UserId,
                IsPresent = this.IsPresent,
                Notes = this.Notes,
            };
        }

    }
}
