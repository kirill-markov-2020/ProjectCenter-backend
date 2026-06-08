using ProjectCenter.Core.Entities;
using ProjectCenter.Core.Enums;

namespace ProjectCenter.Application.Interfaces
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllProjectsAsync();
        Task<List<Project>> GetPublicProjectsAsync();
        Task<List<Project>> GetProjectsByTeacherIdAsync(int teacherId);
        Task<Project?> GetProjectByIdAsync(int id);
        Task AddProjectAsync(Project project);
        Task<Project?> GetActiveProjectByStudentIdAsync(int studentId);
        Task UpdateProjectAsync(Project project);
        Task DeleteProjectAsync(Project project);
        Task<Project?> GetProjectByIdAndTeacherIdAsync(int projectId, int teacherId);
        Task<List<Project>> GetProjectsByStudentIdAsync(int studentId);
        Task<List<Project>> GetAllProjectsWithSortingAsync(ProjectSortBy? sortBy);
        Task<List<Project>> GetPublicProjectsWithSortingAsync(ProjectSortBy? sortBy);

    }
}
