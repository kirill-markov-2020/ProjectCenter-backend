using AutoMapper;
using ProjectCenter.Application.DTOs.Notification;
using ProjectCenter.Application.Interfaces;
using ProjectCenter.Core.Entities;
using ProjectCenter.Core.Exceptions;

    

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
        public async Task SendAddGradeNotificationAsyns(int studentUserId, string teacherFullName, string projectTitle, int gradeValue, string? gradeComment)
        {
            var text = $"Ваш куратор {teacherFullName} оценил ваш проект \"{projectTitle}\" на {gradeValue} баллов.";
            if (!string.IsNullOrWhiteSpace(gradeComment))
                text += $" Комментарий: {gradeComment}";
            var notification = new Notification
            {
                RecipientId = studentUserId,
                Title = "Преподаватель выставил оценку за проект",
                Text = text,
                CreatedAt = DateTime.Now,
                IsRead = false,
                TypeId = 1
            };
            await _notificationRepository.AddAsync(notification);

        }
        public async Task SendUpdateGradeNotificationAsyns(int studentUserId, string teacherFullName, string projectTitle, int oldGradeValue, int newGradeValue, string? gradeComment)
        {
            var text = $"Ваш куратор {teacherFullName} обновил оценку вашего проекта \"{projectTitle}\" с {oldGradeValue} на {newGradeValue} баллов.";
            if (!string.IsNullOrWhiteSpace(gradeComment))
                text += $" Комментарий: {gradeComment}";
            var notification = new Notification
            {
                RecipientId = studentUserId,
                Title = "Преподаватель обновил оценку за проект",
                Text = text,
                CreatedAt = DateTime.Now,
                IsRead = false,
                TypeId = 2
            };
            await _notificationRepository.AddAsync(notification);
        }
        public async Task SendCommentNotificationAsync(int studentUserId, string teacherFullName, string projectTitle, string textPreview)
        {
            var text = $"Ваш куратор {teacherFullName} оставил комментарий к вашему проекту {projectTitle}: {textPreview}";
            var notification = new Notification
            {
                RecipientId = studentUserId,
                Title = "Преподаватель оставил комментарий к проекту",
                Text = text,
                CreatedAt = DateTime.Now,
                IsRead = false,
                TypeId = 3
            };
            await _notificationRepository.AddAsync(notification);
        }
        public async Task SendAddNewProjectNotificationAsync(int curatorId, string studentFullName, string projectTitle, List<int> adminUserIds)
        {

            var text = $"Студент {studentFullName} создал новый проект {projectTitle}. Требуется ваше внимание.";
            var recipientsIds = new List<int> { curatorId};
            recipientsIds.AddRange(adminUserIds);
            foreach(var recipientId in recipientsIds)
            {
                var notification = new Notification
                {
                    RecipientId = recipientId,
                    Title = "Студент создал новый проект",
                    Text = text,
                    CreatedAt = DateTime.Now,
                    IsRead = false,
                    TypeId = 4
                };

                await _notificationRepository.AddAsync(notification);
            }


        }
        public async Task SendDeleteProjectForCuratorNotificationAsync(int curatorUserId, string studentFullName, string projectTitle)
        {

            var text = $"Проект \"{projectTitle}\", автор которого {studentFullName} был удалён администратором.";
            
            var notification = new Notification
            {
                RecipientId = curatorUserId,
                Title = "Администратор удалил проект",
                Text = text,
                CreatedAt = DateTime.Now,
                IsRead = false,
                TypeId = 8
            };

            await _notificationRepository.AddAsync(notification);
            


        }
        public async Task SendDeleteProjectForStudentNotificationAsync(int studentUserId, string projectTitle)
        {

            var text = $"Ваш проект \"{projectTitle}\" был удалён администратором.";

            var notification = new Notification
            {
                RecipientId = studentUserId,
                Title = "Администратор удалил проект",
                Text = text,
                CreatedAt = DateTime.Now,
                IsRead = false,
                TypeId = 8
            };

            await _notificationRepository.AddAsync(notification);



        }
        public async Task SendProjectFileUpdatedNotificationAsync(int curatorUserId, string studentFullName, string projectTitle)
        {
            var text = $"Студент {studentFullName} загрузил новую версию файла проекта \"{projectTitle}\".";

            var notification = new Notification
            {
                RecipientId = curatorUserId,
                Title = "Файл проекта обновлён",
                Text = text,
                CreatedAt = DateTime.Now,
                IsRead = false,
                TypeId = 9
            };

            await _notificationRepository.AddAsync(notification);
        }
        public async Task SendProjectDocumentationUpdatedNotificationAsync(int curatorUserId, string studentFullName, string projectTitle)
        {
            var text = $"Студент {studentFullName} загрузил новую версию документации проекта \"{projectTitle}\".";

            var notification = new Notification
            {
                RecipientId = curatorUserId,
                Title = "Документация проекта обновлена",
                Text = text,
                CreatedAt = DateTime.Now,
                IsRead = false,
                TypeId = 10
            };

            await _notificationRepository.AddAsync(notification);
        }
        public async Task SendProjectDocumentationAndFileUpdatedNotificationAsync(int curatorUserId, string studentFullName, string projectTitle)
        {
            var text = $"Студент {studentFullName} загрузил новые версии файла и документации проекта \"{projectTitle}\".";

            var notification = new Notification
            {
                RecipientId = curatorUserId,
                Title = "Файл и документация проекта обновлены",
                Text = text,
                CreatedAt = DateTime.Now,
                IsRead = false,
                TypeId = 9
            };

            await _notificationRepository.AddAsync(notification);
        }
        public async Task SendProjectVisibilityChangedNotificationAsync(int curatorUserId, string studentFullName, string projectTitle, bool isPublic, List<int> adminUserIds)
        {
            var visibilityText = isPublic ? "публичный" : "приватный";
            var text = $"Студент {studentFullName} изменил видимость проекта \"{projectTitle}\" на \"{visibilityText}\".";

            
            var recipientsIds = new List<int> { curatorUserId };
            recipientsIds.AddRange(adminUserIds);
            foreach (var recipientId in recipientsIds)
            {
                var notification = new Notification
                {
                    RecipientId = recipientId,
                    Title = "Видимость проекта изменена",
                    Text = text,
                    CreatedAt = DateTime.Now,
                    IsRead = false,
                    TypeId = 11
                };

                await _notificationRepository.AddAsync(notification);
            }
            
        }
        public async Task SendProjectTitleChangedNotificationAsync(int studentUserId, string curatorFullName, string newTitle, string oldTitle)
        {
            var text = $"Ваш куратор {curatorFullName} изменил название вашего проекта \"{oldTitle}\" на \"{newTitle}\".";

            var notification = new Notification
            {
                RecipientId = studentUserId,
                Title = "Преподаватель изменил название проекта",
                Text = text,
                CreatedAt = DateTime.Now,
                IsRead = false,
                TypeId = 12
            };

            await _notificationRepository.AddAsync(notification);
        }
        public async Task SendProjectTypeChangedNotificationAsync(int studentUserId, string curatorFullName, string newType, string oldType, string projectTitle)
        {
            var text = $"Ваш куратор {curatorFullName} изменил тип с \"{oldType}\" на \"{newType}\" вашего проекта \"{projectTitle}\".";

            var notification = new Notification
            {
                RecipientId = studentUserId,
                Title = "Преподаватель изменил тип проекта",
                Text = text,
                CreatedAt = DateTime.Now,
                IsRead = false,
                TypeId = 12
            };

            await _notificationRepository.AddAsync(notification);
        }
        public async Task SendProjectSubjectChangedNotificationAsync(int studentUserId, string curatorFullName, string newSubject, string oldSubject, string projectTitle)
        {
            var text = $"Ваш куратор {curatorFullName} изменил предмет с \"{oldSubject}\" на \"{newSubject}\" вашего проекта \"{projectTitle}\".";

            var notification = new Notification
            {
                RecipientId = studentUserId,
                Title = "Преподаватель изменил предмет по которому пишется проект",
                Text = text,
                CreatedAt = DateTime.Now,
                IsRead = false,
                TypeId = 12
            };

            await _notificationRepository.AddAsync(notification);
        }
        public async Task SendProjectDeadlineChangedNotificationAsync(int studentUserId, string curatorFullName, string projectTitle, DateTime newDeadline)
        {
            var text = $"Куратор {curatorFullName} изменил дедлайн вашего проекта \"{projectTitle}\" на {newDeadline:dd.MM.yyyy}.";

            var notification = new Notification
            {
                RecipientId = studentUserId,
                Title = "Дедлайн проекта изменён",
                Text = text,
                CreatedAt = DateTime.Now,
                IsRead = false,
                TypeId = 13
            };

            await _notificationRepository.AddAsync(notification);
        }
        public async Task SendStudentCuratorChangedForOldCuratorNotificationAsync(int oldCuratorUserId, string studentFullName)
        {
            var text = $"Студент {studentFullName} переведён от вас к другому куратору.";

            var notification = new Notification
            {
                RecipientId = oldCuratorUserId,
                Title = "Студент переведён",
                Text = text,
                CreatedAt = DateTime.Now,
                IsRead = false,
                TypeId = 16
            };

            await _notificationRepository.AddAsync(notification);
        }
        public async Task SendStudentCuratorChangedForNewCuratorNotificationAsync(int newCuratorUserId, string studentFullName)
        {
            var text = $"К вам переведён студент {studentFullName}.";

            var notification = new Notification
            {
                RecipientId = newCuratorUserId,
                Title = "Переведён новый студент",
                Text = text,
                CreatedAt = DateTime.Now,
                IsRead = false,
                TypeId = 15
            };

            await _notificationRepository.AddAsync(notification);
        }
        public async Task SendStudentCuratorChangedForStudentNotificationAsync(int studentUserId, string oldCuratorFullName, string newCuratorFullName)
        {
            var text = $"Ваш куратор {oldCuratorFullName} изменён. С этого момента ваш куратор {newCuratorFullName}.";

            var notification = new Notification
            {
                RecipientId = studentUserId,
                Title = "Куратор изменён",
                Text = text,
                CreatedAt = DateTime.Now,
                IsRead = false,
                TypeId = 17
            };

            await _notificationRepository.AddAsync(notification);
        }


    }
}

