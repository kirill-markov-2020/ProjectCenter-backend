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
        Task<List<Project>> GetProjectsByYearAsync(int year);

        Task<List<Project>> GetProjectsFilteredAsync(
            string? searchTerm = null,
            int? year = null,
            int? groupId = null,
            ProjectSortBy? sortBy = null);

        Task<List<Project>> GetPublicProjectsFilteredAsync(
            string? searchTerm = null,
            int? year = null,
            int? groupId = null,
            ProjectSortBy? sortBy = null);

    }
}
