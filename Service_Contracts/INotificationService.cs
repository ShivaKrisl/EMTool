using DTOs.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Contracts
{
    public interface INotificationService
    {
        /// <summary>
        /// Create a new notification
        /// </summary>
        /// <param name="notificationRequest"></param>
        /// <returns></returns>
        public Task<NotificationResponse> CreateNotification(NotificationRequest notificationRequest);

        /// <summary>
        /// Get all notifications of a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<List<NotificationResponse>?> GetAllNotificationsOfUser(Guid userId);

        /// <summary>
        /// Mark a notification as read
        /// </summary>
        /// <param name="notificationRequest"></param>
        /// <returns></returns>
        public Task<bool> MarkNotificationAsRead(Guid notificationId);

        /// <summary>
        /// Delete a notification
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        Task<bool> DeleteNotification(Guid notificationId);

        /// <summary>
        /// Delete all notifications of a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> DeleteAllNotifications(Guid userId);

        /// <summary>
        /// Get unread notification count of a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> GetUnreadNotificationCount(Guid userId);

        /// <summary>
        /// Send bulk notifications
        /// </summary>
        /// <param name="notifications"></param>
        /// <returns></returns>
        Task<bool> SendBulkNotifications(List<NotificationRequest> notifications);
    }
}
