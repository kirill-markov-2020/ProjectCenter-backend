using ProjectCenter.Application.DTOs.Privacy;

namespace ProjectCenter.Application.Interfaces
{
    public interface IPrivacyService
    {
        Task<List<DataStorageCategoryDto>> GetDataStorageSummaryAsync();
    }
}
