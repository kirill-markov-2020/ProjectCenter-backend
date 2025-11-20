using Microsoft.EntityFrameworkCore;
using ProjectCenter.Application.Interfaces;
using ProjectCenter.Core.Entities;
using ProjectCenter.Infrastructure.Persistence.Contexts;

namespace ProjectCenter.Infrastructure.Persistence.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;

        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Project>> GetAllProjectsAsync()
        {
            return await _context.Projects
                .Include(p => p.Student)
                    .ThenInclude(s => s.User)
                .Include(p => p.Teacher)
                    .ThenInclude(t => t.User)
                .Include(p => p.Status)
                .Include(p => p.Type)
                .Include(p => p.Subject)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .ToListAsync();
        }

        public async Task<List<Project>> GetPublicProjectsAsync()
        {
            return await _context.Projects
                .Where(p => p.IsPublic)
                .Include(p => p.Student)
                    .ThenInclude(s => s.User)
                .Include(p => p.Teacher)
                    .ThenInclude(t => t.User)
                .Include(p => p.Status)
                .Include(p => p.Type)
                .Include(p => p.Subject)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .ToListAsync();
        }

        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.Student)
                    .ThenInclude(s => s.User)
                .Include(p => p.Teacher)
                    .ThenInclude(t => t.User)
                .Include(p => p.Status)
                .Include(p => p.Type)
                .Include(p => p.Subject)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddProjectAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
        }
        public async Task<Project?> GetActiveProjectByStudentIdAsync(int studentId)
        {
            
            var inactiveStatuses = new[] { 10, 11, 12, 13, 17, 18 };

            return await _context.Projects
                .Where(p => p.StudentId == studentId && !inactiveStatuses.Contains(p.StatusId))
                .FirstOrDefaultAsync();
        }
        public async Task UpdateProjectAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }
    }
}
