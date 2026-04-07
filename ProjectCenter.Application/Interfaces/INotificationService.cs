using ProjectCenter.Application.DTOs.Notification;

namespace ProjectCenter.Application.Interfaces
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetUserNotificationsAsync(int userId);
        Task<List<NotificationDto>> GetUnreadNotificationsAsync(int userId);
        Task<UnreadCountDto> GetUnreadCountAsync(int userId);
        Task MarkAsReadAsync(int userId, int notificationId);
        Task MarkAllAsReadAsync(int userId);
        Task DeleteNotificationAsync(int userId, int notificationId);
    }
}
