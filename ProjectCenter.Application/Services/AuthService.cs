using ProjectCenter.Application.Interfaces;
using ProjectCenter.Application.DTOs.Auth;
using ProjectCenter.Core.Entities;

namespace ProjectCenter.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repository;
        private readonly IJwtService _jwtService;

        public AuthService(IAuthRepository repository, IJwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _repository.GetUserByLoginAndPasswordAsync(request.Login, request.Password);
            if (user == null)
                throw new UnauthorizedAccessException("Неверный логин или пароль");

            var token = _jwtService.GenerateToken(user);

            return new LoginResponseDto
            {
                Token = token,
                Role = user.IsAdmin ? "Admin" : "User",
                FullName = $"{user.Surname} {user.Name}"
            };
        }
    }
}
