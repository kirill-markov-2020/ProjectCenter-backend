using ProjectCenter.Application.DTOs.Auth;
using ProjectCenter.Application.Interfaces;
using ProjectCenter.Core.Exceptions;

namespace ProjectCenter.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtService _jwtService;

        public AuthService(IAuthRepository authRepository, IJwtService jwtService)
        {
            _authRepository = authRepository;
            _jwtService = jwtService;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await _authRepository.GetUserByLoginAsync(dto.Login);

            if (user == null)
                throw new InvalidEmailException("Пользователь с таким логином не найден.");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                throw new InvalidPasswordException("Неверный пароль.");

            var token = _jwtService.GenerateToken(user);

            return new LoginResponseDto
            {
                Token = token,
                
                Role = user.IsAdmin
                    ? "Admin"
                    : user.Teacher != null
                        ? "Teacher"
                        : user.Student != null
                            ? "Student"
                            : "User",
                FullName = user.Surname + " " + user.Name + " " + user.Patronymic
            };
        }
    }
}
