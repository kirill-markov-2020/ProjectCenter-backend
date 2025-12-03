using Microsoft.EntityFrameworkCore;
using ProjectCenter.Application.Interfaces;
using ProjectCenter.Core.Entities;
using ProjectCenter.Infrastructure.Persistence.Contexts;

namespace ProjectCenter.Infrastructure.Persistence.Repositories
{
    public class DirectoryRepository : IDirectoryRepository
    {
        private readonly AppDbContext _context;

        public DirectoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<StatusProject>> GetStatusesAsync()
            => await _context.StatusProjects.ToListAsync();

        public async Task<List<TypeProject>> GetTypesAsync()
            => await _context.TypeProjects.ToListAsync();

        public async Task<List<Subject>> GetSubjectsAsync()
            => await _context.Subjects.ToListAsync();

        public async Task<List<Group>> GetGroupsAsync()
            => await _context.Groups.ToListAsync();
    }
}
