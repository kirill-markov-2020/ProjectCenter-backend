using ProjectCenter.Application.DTOs.Admin;

namespace ProjectCenter.Application.Interfaces
{
    public interface IAdminService
    {
        Task<AdminDashboardResponse> GetDashboardAsync();
        Task<List<LastProjectDto>> GetLastProjectsAsync(int count = 5);
        Task<List<ActiveTeacherDto>> GetActiveTeachersAsync();
        Task<List<RecentActivityDto>> GetRecentActivityAsync(int count = 5);
    }
}
