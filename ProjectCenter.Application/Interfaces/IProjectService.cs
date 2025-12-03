using ProjectCenter.Application.DTOs;
using ProjectCenter.Application.DTOs.UpdateProject;


public interface IProjectService
{
    Task<List<ProjectDto>> GetProjectsForUserAsync(int userId, bool isAdmin);
    Task<ProjectDto> GetProjectByIdAsync(int id);

    
    Task<ProjectDto> CreateProjectAsync(CreateProjectRequestDto dto, int studentUserId);
    Task<ProjectDto> UpdateProjectAsync(int projectId, UpdateProjectRequestDto dto);
    Task<ProjectDto> UpdateStudentProjectAsync(int projectId, UpdateStudentProjectRequestDto dto, int studentUserId);
    Task DeleteProjectAsync(int projectId);
    Task<ProjectDto?> GetMyProjectAsync(int studentUserId);

}