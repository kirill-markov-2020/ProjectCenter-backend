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

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await _repository.GetUserByLoginAndPasswordAsync(dto.Login, dto.Password);

            if (user == null)
                throw new UnauthorizedAccessException("Неверный логин или пароль.");

            var token = _jwtService.GenerateToken(user);

            string role;
            if (user.IsAdmin)
                role = "Admin";
            else if (user.Teacher != null)
                role = "Teacher";
            else if (user.Student != null)
                role = "Student";
            else
                role = "User";

            return new LoginResponseDto
            {
                Token = token,
                Role = role,
                FullName = $"{user.Surname} {user.Name}"
            };
        }

    }
}
