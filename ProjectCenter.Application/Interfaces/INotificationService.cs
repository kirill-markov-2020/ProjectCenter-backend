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
        Task SendAddGradeNotificationAsyns(int studentUserId, string teacherFullName, string projectTitle, int gradeValue, string? gradeComment);
        Task SendUpdateGradeNotificationAsyns(int studentUserId, string teacherFullName, string projectTitle, int oldGradeValue, int newGradeValue, string? gradeComment);
        Task SendCommentNotificationAsync(int studentUserId, string teacherFullName, string projectTitle, string comment);
        Task SendAddNewProjectNotificationAsync(int userRecipientId, string studentFullName, string projectTitle, List<int> adminUserIds);
        Task SendDeleteProjectForCuratorNotificationAsync(int curatorUserId, string studentFullName, string projectTitle);
        Task SendDeleteProjectForStudentNotificationAsync(int studentUserId, string projectTitle);
        Task SendProjectFileUpdatedNotificationAsync(int curatorUserId, string studentFullName, string projectTitle);
        Task SendProjectDocumentationUpdatedNotificationAsync(int curatorUserId, string studentFullName, string projectTitle);
        Task SendProjectDocumentationAndFileUpdatedNotificationAsync(int curatorUserId, string studentFullName, string projectTitle);
        Task SendProjectVisibilityChangedNotificationAsync(int curatorUserId, string studentFullName, string projectTitle, bool isPublic, List<int> adminUserIds);
        Task SendProjectTitleChangedNotificationAsync(int studentUserId, string curatorFullName, string newTitle, string oldTitle);
        Task SendProjectTypeChangedNotificationAsync(int studentUserId, string curatorFullName, string newType, string oldType, string projectTitle);
        Task SendProjectSubjectChangedNotificationAsync(int studentUserId, string curatorFullName, string newSubject, string oldSubject, string projectTitle);
        Task SendProjectDeadlineChangedNotificationAsync(int studentUserId, string curatorFullName, string projectTitle, DateTime newDeadline);
        Task SendStudentCuratorChangedForOldCuratorNotificationAsync(int oldCuratorUserId, string studentFullName);
        Task SendStudentCuratorChangedForNewCuratorNotificationAsync(int newCuratorUserId, string studentFullName);
        Task SendStudentCuratorChangedForStudentNotificationAsync(int studentUserId, string oldCuratorFullName, string newCuratorFullName);
        Task SendUserWelcomeNotificationAsync(int userId, string userFullName, string role);
        Task SendNewTeacherNotificationForAdminsAsync(List<int> adminUserIds, string teacherFullName, string teacherEmail);
        Task SendNewStudentNotificationForAdminsAsync(List<int> adminUserIds, string studentFullName, string studentEmail);

        Task SendStudentDeletedNotificationForCuratorAsync(int curatorUserId, string studentFullName, string groupName);
    }
}
