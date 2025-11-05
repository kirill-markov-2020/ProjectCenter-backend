using Microsoft.EntityFrameworkCore;
using ProjectCenter.Application.DTOs.Auth;
using ProjectCenter.Application.Interfaces;
using ProjectCenter.Core.Entities;
using ProjectCenter.Infrastructure.Persistance.Contexts;
using ProjectCenter.Infrastructure.Services;
using System;

namespace ProjectCenter.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AuthService(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Login == request.Login && u.Password == request.Password);

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
