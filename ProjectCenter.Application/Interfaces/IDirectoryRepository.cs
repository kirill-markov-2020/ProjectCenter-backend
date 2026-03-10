using ProjectCenter.Core.Entities;

namespace ProjectCenter.Application.Interfaces
{
    public interface IDirectoryRepository
    {
        Task<List<StatusProject>> GetStatusesAsync();
        Task<List<TypeProject>> GetTypesAsync();
        Task<List<Subject>> GetSubjectsAsync();
        Task<List<Group>> GetGroupsAsync();
    }
}
