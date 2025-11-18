using ProjectCenter.Application.DTOs;

namespace ProjectCenter.Application.Interfaces
{
    public interface IProjectService
    {
        Task<List<ProjectDto>> GetProjectsForUserAsync(int userId, bool isAdmin);
        Task<ProjectDto> GetProjectByIdAsync(int id);
    }
}
