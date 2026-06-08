using ProjectCenter.Application.DTOs.Project;
using ProjectCenter.Core.Enums;


public interface IProjectService
{
    Task<List<ProjectDto>> GetProjectsForUserAsync(int userId, ProjectSortBy? sortBy = null);
    Task<ProjectDto> GetProjectByIdAsync(int id);

    
    Task<ProjectDto> CreateProjectAsync(CreateProjectRequestDto dto, int studentUserId);
    Task<ProjectDto> UpdateProjectAsync(int projectId, UpdateProjectRequestDto dto);
    Task<ProjectDto> UpdateStudentProjectAsync(int projectId, UpdateStudentProjectRequestDto dto, int studentUserId);
    Task DeleteProjectAsync(int projectId);
    Task<ProjectDto?> GetMyProjectAsync(int studentUserId);
    Task AddCommentAsync(int projectId, int userId, string text);
    Task<ProjectDto> GetTeacherStudentProjectAsync(int projectId, int teacherUserId);
    Task<ProjectDto> UpdateProjectByTeacherAsync(int projectId, UpdateTeacherProjectRequestDto dto, int teacherUserId);

}