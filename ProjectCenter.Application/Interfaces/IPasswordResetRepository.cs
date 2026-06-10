using ProjectCenter.Core.Entities;

namespace ProjectCenter.Application.Interfaces
{
    public interface IPasswordResetRepository
    {
        Task AddAsync(PasswordResetCode resetCode);
        Task<PasswordResetCode?> GetByEmailAndCodeAsync(string email, string code);
        Task UpdateAsync(PasswordResetCode resetCode);
        Task MarkAsUsedAsync(int id);
        Task RemoveExpiredCodesAsync();
    }
}