using Microsoft.EntityFrameworkCore;
using ProjectCenter.Application.Interfaces;
using ProjectCenter.Core.Entities;
using ProjectCenter.Infrastructure.Persistence.Contexts;

namespace ProjectCenter.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task AddTeacherAsync(Teacher teacher)
        {
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();
        }

        public async Task AddStudentAsync(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> LoginExistsAsync(string login)
        {
            return await _context.Users.AnyAsync(u => u.Login == login);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Teacher)
                .Include(u => u.Student)
                    .ThenInclude(s => s.Group)
                .Include(u => u.Student)
                    .ThenInclude(g => g.Teacher)
                .ToListAsync();
        }


    }
}
