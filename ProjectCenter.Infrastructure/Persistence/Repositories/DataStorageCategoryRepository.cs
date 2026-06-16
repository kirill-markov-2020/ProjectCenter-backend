using Microsoft.EntityFrameworkCore;
using ProjectCenter.Application.Interfaces;
using ProjectCenter.Core.Entities;
using ProjectCenter.Infrastructure.Persistence.Contexts;

namespace ProjectCenter.Infrastructure.Persistence.Repositories
{
    public class DataStorageCategoryRepository : IDataStorageCategoryRepository
    {
        private readonly AppDbContext _context;

        public DataStorageCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DataStorageCategory>> GetAllAsync()
            => await _context.DataStorageCategories.ToListAsync();
    }
}
