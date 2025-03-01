using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfCore
{
    public class Notification
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
        public bool IsRead { get; set; } = false;

        // Navigation Property
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
