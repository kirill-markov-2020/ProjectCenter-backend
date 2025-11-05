using ProjectCenter.Application.DTOs.Auth;

namespace ProjectCenter.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }
}
