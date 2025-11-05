using ProjectCenter.Core.Entities;

namespace ProjectCenter.Application.Interfaces
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByLoginAndPasswordAsync(string login, string password);
    }
}
