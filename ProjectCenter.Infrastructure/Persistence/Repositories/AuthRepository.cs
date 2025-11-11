using ProjectCenter.Application.Interfaces;
using ProjectCenter.Core.Entities;
using ProjectCenter.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ProjectCenter.Infrastructure.Persistence.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByLoginAndPasswordAsync(string login, string password)
        {
            var user = await _context.Users
                .Include(u => u.Teacher)
                .Include(u => u.Student)
                .FirstOrDefaultAsync(u => u.Login == login);

            if (user == null)
                return null;

            if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                return user;

            return null;
        }
    }
}
