using ProjectCenter.Application.DTOs.Directory;

namespace ProjectCenter.Application.Interfaces
{
    public interface IDirectoryService
    {
        Task<List<StatusProjectDto>> GetStatusesAsync();
        Task<List<TypeProjectDto>> GetTypesAsync();
        Task<List<SubjectDto>> GetSubjectsAsync();
        Task<List<GroupDto>> GetGroupsAsync();
    }
}
