using ProjectCenter.Application.DTOs;


public interface IProjectService
{
    Task<List<ProjectDto>> GetProjectsForUserAsync(int userId, bool isAdmin);
    Task<ProjectDto> GetProjectByIdAsync(int id);

    
    Task<ProjectDto> CreateProjectAsync(CreateProjectRequestDto dto, int studentUserId);
    Task<ProjectDto> UpdateProjectAsync(int projectId, UpdateProjectRequestDto dto);
}