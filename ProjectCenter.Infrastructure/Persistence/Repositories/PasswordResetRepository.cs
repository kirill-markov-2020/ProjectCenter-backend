using Microsoft.EntityFrameworkCore;
using ProjectCenter.Application.Interfaces;
using ProjectCenter.Core.Entities;
using ProjectCenter.Infrastructure.Persistence.Contexts;

namespace ProjectCenter.Infrastructure.Persistence.Repositories
{
    public class PasswordResetRepository : IPasswordResetRepository
    {
        private readonly AppDbContext _context;

        public PasswordResetRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PasswordResetCode resetCode)
        {
            await _context.PasswordResetCodes.AddAsync(resetCode);
            await _context.SaveChangesAsync();
        }

        public async Task<PasswordResetCode?> GetByEmailAndCodeAsync(string email, string code)
        {
            return await _context.PasswordResetCodes
                .FirstOrDefaultAsync(x => x.Email == email && x.Code == code && !x.IsUsed);
        }

        public async Task UpdateAsync(PasswordResetCode resetCode)
        {
            _context.PasswordResetCodes.Update(resetCode);
            await _context.SaveChangesAsync();
        }

        public async Task MarkAsUsedAsync(int id)
        {
            var code = await _context.PasswordResetCodes.FindAsync(id);
            if (code != null)
            {
                code.IsUsed = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveExpiredCodesAsync()
        {
            var expiredCodes = await _context.PasswordResetCodes
                .Where(x => x.ExpiresAt < DateTime.Now || x.IsUsed)
                .ToListAsync();

            _context.PasswordResetCodes.RemoveRange(expiredCodes);
            await _context.SaveChangesAsync();
        }
    }
}