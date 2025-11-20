using ProjectCenter.Core.Entities;

public interface IAuthRepository
{
    Task<User?> GetUserByLoginAsync(string login);
}
