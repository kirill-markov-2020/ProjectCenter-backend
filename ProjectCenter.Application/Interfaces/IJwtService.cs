namespace ProjectCenter.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(Core.Entities.User user);
    }
}
