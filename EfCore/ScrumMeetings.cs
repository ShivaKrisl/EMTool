using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCore
{
    public class ScrumMeetings
    {
        /// <summary>
        /// Id of Scrum Meeting
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Date of Scrum Meeting
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

        // Navigation Properties

        [ForeignKey("TeamId")]
        public Teams team { get; set; }

        public ICollection<ScrumAttendance> scrumAttendances { get; set; } = new List<ScrumAttendance>();
    }
}
