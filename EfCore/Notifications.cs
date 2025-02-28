using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EfCore
{
    public class Notifications
    {
        /// <summary>
        /// Notification Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// User Id who will receive the notification
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// Notification Message
        /// </summary>
        [Required]
        public string NotificationMessage { get; set; }

        /// <summary>
        /// Notification Created Date
        /// </summary>
        [Required]
        public DateTime NotificationCreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Notification Type (TaskAdded/TaskUpdated/PRApproved/PRRejected)
        /// </summary>
        [Required]
        public string NotificationType { get; set; }

        /// <summary>
        /// Notification Status (Read/Unread)
        /// </summary>
        public bool IsRead { get; set; } = false;

        // Navigation Properties

        [ForeignKey("UserId")]
        public Users user { get; set; }

    }
}
