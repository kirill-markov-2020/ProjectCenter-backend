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
            return await _context.Users.FirstOrDefaultAsync(u => u.Login == login && u.Password == password);
        }
    }
}
