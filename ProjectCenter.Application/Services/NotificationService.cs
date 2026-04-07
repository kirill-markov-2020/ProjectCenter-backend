namespace ProjectCenter.Application.Services
{
    // ProjectCenter.Application/Services/NotificationService.cs
    using AutoMapper;
    using global::ProjectCenter.Application.DTOs.Notification;
    using global::ProjectCenter.Application.Interfaces;
    using global::ProjectCenter.Core.Exceptions;

    namespace ProjectCenter.Application.Services
    {
        public class NotificationService : INotificationService
        {
            private readonly INotificationRepository _notificationRepository;
            private readonly IMapper _mapper;

            public NotificationService(INotificationRepository notificationRepository, IMapper mapper)
            {
                _notificationRepository = notificationRepository;
                _mapper = mapper;
            }

            public async Task<List<NotificationDto>> GetUserNotificationsAsync(int userId)
            {
                var notifications = await _notificationRepository.GetUserNotificationsAsync(userId);
                return notifications.Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Text = n.Text,
                    CreatedAt = n.CreatedAt,
                    IsRead = n.IsRead,
                    TypeId = n.TypeId,
                    TypeName = n.Type?.Name ?? "System"
                }).ToList();
            }

            public async Task<List<NotificationDto>> GetUnreadNotificationsAsync(int userId)
            {
                var notifications = await _notificationRepository.GetUnreadNotificationsAsync(userId);
                return notifications.Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Text = n.Text,
                    CreatedAt = n.CreatedAt,
                    IsRead = n.IsRead,
                    TypeId = n.TypeId,
                    TypeName = n.Type?.Name ?? "System"
                }).ToList();
            }

            public async Task<UnreadCountDto> GetUnreadCountAsync(int userId)
            {
                var count = await _notificationRepository.GetUnreadCountAsync(userId);
                return new UnreadCountDto { UnreadCount = count };
            }

            public async Task MarkAsReadAsync(int userId, int notificationId)
            {
                var notification = await _notificationRepository.GetByIdAsync(notificationId);

                if (notification == null)
                    throw new Exception("Уведомление не найдено");

                if (notification.RecipientId != userId)
                    throw new AccessDeniedException("Вы не можете отметить это уведомление как прочитанное");

                await _notificationRepository.MarkAsReadAsync(notificationId);
            }

            public async Task MarkAllAsReadAsync(int userId)
            {
                await _notificationRepository.MarkAllAsReadAsync(userId);
            }

            public async Task DeleteNotificationAsync(int userId, int notificationId)
            {
                var notification = await _notificationRepository.GetByIdAsync(notificationId);

                if (notification == null)
                    throw new Exception("Уведомление не найдено");

                if (notification.RecipientId != userId)
                    throw new AccessDeniedException("Вы не можете удалить это уведомление");

                await _notificationRepository.DeleteAsync(notificationId);
            }
        }
    }
}
