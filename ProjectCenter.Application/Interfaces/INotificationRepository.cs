using ProjectCenter.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCenter.Application.Interfaces
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetUserNotificationsAsync(int userId);
        Task<List<Notification>> GetUnreadNotificationsAsync(int userId);
        Task<Notification?> GetByIdAsync(int id);
        Task<int> GetUnreadCountAsync(int userId);
        Task AddAsync(Notification notification);
        Task UpdateAsync(Notification notification);
        Task MarkAsReadAsync(int notificationId);
        Task MarkAllAsReadAsync(int userId);
        Task DeleteAsync(int id);
    }
}
