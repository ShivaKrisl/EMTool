using EfCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Notifications
{
    public class NotificationResponse
    {
        // <summary>
        /// Notification Id
        /// </summary>
        [Required]
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

        // Navigation Property
        [Required]
        public User User { get; set; }
    }

    public static class NotificationResponseExtensions
    {
        public static NotificationResponse ToNotificationResponse(this Notification notification)
        {
            return new NotificationResponse()
            {
                Id = notification.Id,
                UserId = notification.UserId,
                NotificationMessage = notification.NotificationMessage,
                CreatedAt = notification.CreatedAt,
                NotificationType = notification.NotificationType,
                IsRead = notification.IsRead,
                User = notification.User
            };
        }
    }

}
