using ProjectCenter.Core.Entities;

namespace ProjectCenter.Application.Interfaces
{
    public interface IDataStorageCategoryRepository
    {
        Task<List<DataStorageCategory>> GetAllAsync();
    }
}
