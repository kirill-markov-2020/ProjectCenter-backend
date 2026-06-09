using ProjectCenter.Application.DTOs.Directory;
using ProjectCenter.Application.DTOs.Project;
using ProjectCenter.Core.Enums;


public interface IProjectService
{
    Task<List<ProjectDto>> GetProjectsForUserAsync(
        int userId,
        string? searchTerm = null,
        int? year = null,
        int? groupId = null,
        ProjectSortBy? sortBy = null);

    Task<List<GroupDto>> GetAvailableGroupsForYearAsync(int year);
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