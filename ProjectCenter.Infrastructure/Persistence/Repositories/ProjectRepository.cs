using Microsoft.EntityFrameworkCore;
using ProjectCenter.Application.Interfaces;
using ProjectCenter.Core.Entities;
using ProjectCenter.Core.Enums;
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
                .Include(p => p.Student)
                    .ThenInclude(s => s.Group)
                .Include(p => p.Teacher)
                    .ThenInclude(t => t.User)
                .Include(p => p.Status)
                .Include(p => p.Type)
                .Include(p => p.Subject)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(p => p.Grade)
                    .ThenInclude(g => g.Teacher)
                        .ThenInclude(t => t.User)
                .ToListAsync();
        }

        public async Task<List<Project>> GetPublicProjectsAsync()
        {
            return await _context.Projects
                .Where(p => p.IsPublic)
                .Include(p => p.Student)
                    .ThenInclude(s => s.User)
                .Include(p => p.Student)
                    .ThenInclude(s => s.Group)
                .Include(p => p.Teacher)
                    .ThenInclude(t => t.User)
                .Include(p => p.Status)
                .Include(p => p.Type)
                .Include(p => p.Subject)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(p => p.Grade)
                    .ThenInclude(g => g.Teacher)
                        .ThenInclude(t => t.User)
                .ToListAsync();
        }
      
        public async Task<List<Project>> GetProjectsByTeacherIdAsync(int teacherId)
        {
            return await _context.Projects
                .Where(p => p.TeacherId == teacherId)
                .Include(p => p.Student)
                    .ThenInclude(s => s.User)
                .Include(p => p.Student)
                    .ThenInclude(s => s.Group)
                .Include(p => p.Teacher)
                    .ThenInclude(t => t.User)
                .Include(p => p.Status)
                .Include(p => p.Type)
                .Include(p => p.Subject)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(p => p.Grade)
                    .ThenInclude(g => g.Teacher)
                        .ThenInclude(t => t.User)
                .ToListAsync();
        }

        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.Student)
                    .ThenInclude(s => s.User)
                .Include(p => p.Student)
                    .ThenInclude(s => s.Group)
                .Include(p => p.Teacher)
                    .ThenInclude(t => t.User)
                .Include(p => p.Status)
                .Include(p => p.Type)
                .Include(p => p.Subject)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(p => p.Grade)
                    .ThenInclude(g => g.Teacher)
                        .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task AddProjectAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
        }
        public async Task<Project?> GetActiveProjectByStudentIdAsync(int studentId)
        {
            
            var inactiveStatuses = new[] {5,6, 7};

            return await _context.Projects
                .Where(p => p.StudentId == studentId && !inactiveStatuses.Contains(p.StatusId))
                .FirstOrDefaultAsync();
        }
        public async Task UpdateProjectAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteProjectAsync(Project project)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
  
        public async Task<Project?> GetProjectByIdAndTeacherIdAsync(int projectId, int teacherId)
        {
            return await _context.Projects
                .Where(p => p.Id == projectId && p.TeacherId == teacherId)
                .Include(p => p.Student)
                    .ThenInclude(s => s.User)
                .Include(p => p.Student)
                    .ThenInclude(s => s.Group)
                .Include(p => p.Teacher)
                    .ThenInclude(t => t.User)
                .Include(p => p.Status)
                .Include(p => p.Type)
                .Include(p => p.Subject)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(p => p.Grade)
                    .ThenInclude(g => g.Teacher)
                        .ThenInclude(t => t.User)
                .FirstOrDefaultAsync();
        }
        public async Task<List<Project>> GetProjectsByStudentIdAsync(int studentId)
        {
            return await _context.Projects
                .Where(p => p.StudentId == studentId)
                .ToListAsync();
        }
        public async Task<List<Project>> GetProjectsByYearAsync(int year)
        {
            return await _context.Projects
                .Include(p => p.Student)
                    .ThenInclude(s => s.Group)
                .Where(p => p.Year == year)
                .ToListAsync();
        }
        public async Task<List<Project>> GetProjectsFilteredAsync(
            string? searchTerm = null,
            int? year = null,
            int? groupId = null,
            
            ProjectSortBy? sortBy = null)
        {
            var query = _context.Projects
                .Include(p => p.Student)
                    .ThenInclude(s => s.User)
                .Include(p => p.Student)
                    .ThenInclude(s => s.Group)
                .Include(p => p.Teacher)
                    .ThenInclude(t => t.User)
                .Include(p => p.Status)
                .Include(p => p.Type)
                .Include(p => p.Subject)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(p => p.Grade)
                    .ThenInclude(g => g.Teacher)
                        .ThenInclude(t => t.User)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim();
                query = query.Where(p =>
                    p.Title.Contains(term) ||
                    (p.Student.User.Surname + " " + p.Student.User.Name + " " + p.Student.User.Patronymic).Contains(term) ||
                    (p.Teacher.User.Surname + " " + p.Teacher.User.Name + " " + p.Teacher.User.Patronymic).Contains(term) ||
                    p.Student.Group.Name.Contains(term));
            }

            if (year.HasValue)
                query = query.Where(p => p.Year == year.Value);

            if (groupId.HasValue)
                query = query.Where(p => p.Student.GroupId == groupId.Value);
            query = sortBy switch
            {
                ProjectSortBy.CreatedDateAsc => query.OrderBy(p => p.CreatedDate),
                ProjectSortBy.CreatedDateDesc => query.OrderByDescending(p => p.CreatedDate),
                ProjectSortBy.DeadlineAsc => query.OrderBy(p => p.DateDeadline),
                ProjectSortBy.DeadlineDesc => query.OrderByDescending(p => p.DateDeadline),
                _ => query.OrderByDescending(p => p.CreatedDate)
            };

            return await query.ToListAsync();
        }

        public async Task<List<Project>> GetPublicProjectsFilteredAsync(
            string? searchTerm = null,
            int? year = null,
            int? groupId = null,
            
            ProjectSortBy? sortBy = null)
        {
            var query = _context.Projects
                .Where(p => p.IsPublic)
                .Include(p => p.Student)
                    .ThenInclude(s => s.User)
                .Include(p => p.Student)
                    .ThenInclude(s => s.Group)
                .Include(p => p.Teacher)
                    .ThenInclude(t => t.User)
                .Include(p => p.Status)
                .Include(p => p.Type)
                .Include(p => p.Subject)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(p => p.Grade)
                    .ThenInclude(g => g.Teacher)
                        .ThenInclude(t => t.User)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = searchTerm.Trim();
                query = query.Where(p =>
                    p.Title.Contains(term) ||
                    (p.Student.User.Surname + " " + p.Student.User.Name + " " + p.Student.User.Patronymic).Contains(term) ||
                    (p.Teacher.User.Surname + " " + p.Teacher.User.Name + " " + p.Teacher.User.Patronymic).Contains(term) ||
                    p.Student.Group.Name.Contains(term));
            }

            if (year.HasValue)
                query = query.Where(p => p.Year == year.Value);

            if (groupId.HasValue)
                query = query.Where(p => p.Student.GroupId == groupId.Value);


            query = sortBy switch
            {
                ProjectSortBy.CreatedDateAsc => query.OrderBy(p => p.CreatedDate),
                ProjectSortBy.CreatedDateDesc => query.OrderByDescending(p => p.CreatedDate),
                ProjectSortBy.DeadlineAsc => query.OrderBy(p => p.DateDeadline),
                ProjectSortBy.DeadlineDesc => query.OrderByDescending(p => p.DateDeadline),
                _ => query.OrderByDescending(p => p.CreatedDate)
            };

            return await query.ToListAsync();
        }

    }
}
