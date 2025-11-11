using Azure.Core;
using BCrypt.Net;
using ProjectCenter.Application.DTOs;
using ProjectCenter.Application.DTOs.CreateUser;
using ProjectCenter.Application.Interfaces;
using ProjectCenter.Core.Entities;
using ProjectCenter.Core.Exceptions;
using ProjectCenter.Core.ValueObjects;
using System;
using System.Threading.Tasks;
using System.Linq;

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

            var validRoles = new[] { "Admin", "Teacher", "Student" };
            if (!validRoles.Any(r => roleNormalized.Equals(r, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidRoleException($"Недопустимая роль: {dto.Role}. Разрешены только Admin, Teacher, Student.");

            if (!PhoneValidator.IsValid(dto.Phone))
                throw new InvalidPhoneNumberException("Некорректный формат телефона. Используйте формат +7XXXXXXXXXX или 8XXXXXXXXXX.");

            var emailErrors = EmailValidator.Validate(dto.Email);
            if (emailErrors.Any())
                throw new InvalidEmailException(string.Join(" ", emailErrors));

            var passwordErrors = PasswordValidator.Validate(dto.Password);
            if (passwordErrors.Any())
                throw new InvalidPasswordException(string.Join(" ", passwordErrors));

            if (await _userRepository.LoginExistsAsync(dto.Login))
                throw new ArgumentException("Такой логин уже занят");

            if (await _userRepository.EmailExistsAsync(dto.Email))
                throw new ArgumentException("Такой Email уже занят");

            if (roleNormalized.Equals("Student", StringComparison.OrdinalIgnoreCase))
            {
                if (!dto.GroupId.HasValue || !dto.TeacherId.HasValue)
                    throw new InvalidStudentDataException("Для роли 'Student' необходимо указать GroupId и TeacherId.");
            }
            else
            {
                if ((dto.GroupId.HasValue && dto.GroupId.Value != 0) || (dto.TeacherId.HasValue && dto.TeacherId.Value != 0))
                    throw new InvalidStudentDataException("GroupId и TeacherId можно указывать только для роли 'Student'.");
            }

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

            await _userRepository.AddUserAsync(user);

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
                var student = new Student
                {
                    UserId = user.Id,
                    GroupId = dto.GroupId!.Value,
                    TeacherId = dto.TeacherId!.Value
                };

                await _userRepository.AddStudentAsync(student);
            }

            return new CreateUserResponseDto
            {
                UserId = user.Id,
                Role = roleNormalized
            };
        }
        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();

            var result = users.Select(u => new UserDto
            {
                Id = u.Id,
                FullName = $"{u.Surname} {u.Name} {u.Patronymic}".Trim(),
                Login = u.Login,
                Email = u.Email,
                Phone = u.Phone,
                Role = u.IsAdmin ? "Admin"
                     : u.Teacher != null ? "Teacher"
                     : u.Student != null ? "Student"
                     : "User",
                GroupName = u.Student?.Group?.Name,
                CuratorName = u.Student?.Teacher != null
                    ? $"{u.Student.Teacher.User.Surname} {u.Student.Teacher.User.Name} {u.Student.Teacher.User.Patronymic}".Trim()
                    : null
            }).ToList();

            return result;
        }
        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                throw new UserNotFoundException(id);

            if (user.IsAdmin)
                throw new InvalidOperationException("Нельзя удалить администратора.");

            // Удаление связанных сущностей
            if (user.Student != null)
                await _userRepository.DeleteStudentAsync(user.Student);

            if (user.Teacher != null)
                await _userRepository.DeleteTeacherAsync(user.Teacher);

            await _userRepository.DeleteUserAsync(user);
        }


    }
}
