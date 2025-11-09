using Azure.Core;
using BCrypt.Net;
using ProjectCenter.Application.DTOs;
using ProjectCenter.Application.DTOs.CreateUser;
using ProjectCenter.Application.Interfaces;
using ProjectCenter.Core.Entities;
using System;

namespace ProjectCenter.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<CreateUserResponseDto> CreateUserAsync(CreateUserRequestDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var roleNormalized = dto.Role?.Trim();

            if (string.IsNullOrEmpty(roleNormalized))
                throw new ArgumentException("Role is required.");

            // Проверка уникальности логина/email
            if (await _userRepository.LoginExistsAsync(dto.Login))
                throw new ArgumentException("Login already exists.");

            if (await _userRepository.EmailExistsAsync(dto.Email))
                throw new ArgumentException("Email already exists.");

            // Хэшируем пароль
            var hashed = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            
            var user = new User
            {
                Surname = dto.Surname,
                Name = dto.Name,
                Patronymic = dto.Patronymic,
                Login = dto.Login,
                Password = hashed,
                Phone = dto.Phone,
                Email = dto.Email,
                Photo = dto.Photo,
                IsAdmin = roleNormalized.Equals("Admin", StringComparison.OrdinalIgnoreCase)
            };

            // Сохраняем пользователя первым — чтобы получить Id
            await _userRepository.AddUserAsync(user);

            // В зависимости от роли — создаём дополнительные записи
            if (roleNormalized.Equals("Teacher", StringComparison.OrdinalIgnoreCase))
            {
                var teacher = new Teacher
                {
                    UserId = user.Id
                };

                await _userRepository.AddTeacherAsync(teacher);
            }
            else if (roleNormalized.Equals("Student", StringComparison.OrdinalIgnoreCase))
            {
                if (!dto.GroupId.HasValue || !dto.TeacherId.HasValue)
                    throw new ArgumentException("GroupId and TeacherId are required for role 'Student'.");

                var student = new Student
                {
                    UserId = user.Id,
                    GroupId = dto.GroupId.Value,
                    TeacherId = dto.TeacherId.Value
                };

                await _userRepository.AddStudentAsync(student);
            }

            return new CreateUserResponseDto
            {
                UserId = user.Id,
                Role = roleNormalized
            };
        }
    }
}
