using DTOs.Notifications;
using DTOs.Roles;
using DTOs.Users;
using EfCore;
using Service_Contracts;


namespace Services
{
    public class NotificationService : INotificationService
    {

        private readonly List<Notification> _notifications;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public NotificationService()
        {
            _notifications = new List<Notification>();
            _userService = new UserService();
            _roleService = new RoleService();
        }

        /// <summary>
        /// Create a new notification
        /// </summary>
        /// <param name="notificationRequest"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async Task<NotificationResponse> CreateNotification(NotificationRequest notificationRequest)
        {
            if(notificationRequest == null)
            {
                throw new ArgumentNullException(nameof(notificationRequest));
            }

            bool isValidState = ValidationHelper.IsStateValid(notificationRequest);

            if (!isValidState)
            {
                throw new ArgumentException("Invalid Notification Request");
            }

            UserResponse? userResponse =  await _userService.GetEmployeeById(notificationRequest.UserId);
            if (userResponse == null)
            {
                throw new ArgumentException("User not found");
            }

            RoleResponse? roleResponse =  await _roleService.GetRoleById(userResponse.RoleId);

            if (roleResponse == null)
            {
                throw new ArgumentException("Role not found");
            }

            Notification notification = notificationRequest.ToNotification();
            notification.CreatedAt = DateTime.UtcNow;
            notification.Id = Guid.NewGuid();
            notification.IsRead = false;
            notification.User = userResponse.ToUser(roleResponse.ToRole());

            _notifications.Add(notification);

            return notification.ToNotificationResponse();

        }


        /// <summary>
        /// Get all notifications of a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<List<NotificationResponse>?> GetAllNotificationsOfUser(Guid userId)
        {
            if(userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            List<Notification> notifications = _notifications.Where(n => n.UserId == userId).ToList();

            if (!notifications.Any())
            {
                return Task.FromResult<List<NotificationResponse>?>(null);
            }

            List<NotificationResponse>? notificationResponses = new List<NotificationResponse>();

            foreach (var notification in notifications)
            {
                notificationResponses.Add(notification.ToNotificationResponse());
            }

            return Task.FromResult<List<NotificationResponse>?>(notificationResponses);

        }

        /// <summary>
        /// Get no.of unread notifications
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        Task<int> INotificationService.GetUnreadNotificationCount(Guid userId)
        {
            if(userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            List<Notification>? notifications =  _notifications.Where(n => n.IsRead == false).ToList();

            if (!notifications.Any())
            {
                return Task.FromResult(0);
            }

            return Task.FromResult(notifications.Count());
        }

        /// <summary>
        /// Mark notifications as Read
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<bool> MarkNotificationAsRead(Guid notificationId)
        {
            if(notificationId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(notificationId));
            }

            Notification? notification = _notifications.FirstOrDefault(n => n.Id == notificationId);

            if (notification == null)
            {
                return Task.FromResult(false);
            }

            notification.IsRead = true;
            return Task.FromResult(notification.IsRead);

        }

        /// <summary>
        /// Delete a Notification
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<bool> DeleteNotification(Guid notificationId)
        {
            if(notificationId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(notificationId));
            }

            Notification? notification = _notifications.FirstOrDefault(o => o.Id == notificationId);

            if(notification == null)
            {
                return Task.FromResult(false);
            }

            _notifications.Remove(notification);

            return Task.FromResult(true);

        }

        /// <summary>
        /// Delete all notifications of user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Task<bool> DeleteAllNotifications(Guid userId)
        {
            if(userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            List<Notification>? notifications = _notifications.FindAll(o => o.UserId == userId);

            if(notifications == null || !notifications.Any())
            {
                return Task.FromResult(false);
            }

            _notifications.RemoveAll(o => o.UserId == userId);
            return Task.FromResult(true);
        }


        /// <summary>
        /// Send Bulk Notifications
        /// </summary>
        /// <param name="notifications"></param>
        /// <returns></returns>
        public async Task<bool> SendBulkNotifications(List<NotificationRequest> notifications)
        {
            if (notifications == null)
            {
                throw new ArgumentNullException(nameof(notifications), "Notification list cannot be null.");
            }

            if (!notifications.Any())
            {
                return false; 
            }

            List<Task> notificationTasks = new List<Task>();

            foreach (NotificationRequest notificationRequest in notifications)
            {
                notificationTasks.Add(CreateNotification(notificationRequest));
            }

            try
            {
                await Task.WhenAll(notificationTasks); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending some notifications: {ex.Message}");
            }

            return true; // Even if some fail, return true (notifying some users is better than none)
        }

    }
}
