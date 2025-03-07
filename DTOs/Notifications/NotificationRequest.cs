using EfCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Notifications
{
    public class NotificationRequest
    {
        /// <summary>
        /// User Id who will receive the notification
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// Notification Message
        /// </summary>
        [Required]
        [MaxLength(500)] // Limit to prevent excessive message size
        public string NotificationMessage { get; set; }

        /// <summary>
        /// Notification Created Date
        /// </summary>
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Notification Type (TaskAdded/TaskUpdated/PRApproved/PRRejected)
        /// </summary>
        [Required]
        [MaxLength(50)] // Prevent long and unnecessary values
        public string NotificationType { get; set; }

        /// <summary>
        /// Notification Status (Read/Unread)
        /// </summary>
        public bool IsRead { get; set; }

        public Notification ToNotification()
        {
            return new Notification()
            {
                UserId = this.UserId,
                NotificationMessage = this.NotificationMessage,
                CreatedAt = this.CreatedAt,
                NotificationType = this.NotificationType,
                IsRead = this.IsRead
            };
        }
    }
}
