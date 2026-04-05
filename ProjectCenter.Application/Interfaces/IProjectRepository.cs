using ProjectCenter.Core.Entities;

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

    }
}
