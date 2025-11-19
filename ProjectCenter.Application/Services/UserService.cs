using AutoMapper;
using Azure.Core;
using BCrypt.Net;
using ProjectCenter.Application.DTOs;
using ProjectCenter.Application.DTOs.CreateUser;
using ProjectCenter.Application.DTOs.UpdateUser;
using ProjectCenter.Application.Interfaces;
using ProjectCenter.Core.Entities;
using ProjectCenter.Core.Exceptions;
using ProjectCenter.Core.ValueObjects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectCenter.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public UserService(IUserRepository userRepository, IMapper mapper, IFileService fileService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _fileService = fileService;
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
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new ArgumentException("Пользователь не найден.");

          
            if (user.Teacher != null)
            {
                var students = await _userRepository.GetAllAsync();
                bool hasStudents = students.Any(s => s.Student != null && s.Student.TeacherId == user.Teacher.Id);

                if (hasStudents)
                    throw new TeacherHasStudentsException();

                await _userRepository.DeleteTeacherAsync(user.Teacher);
            }
            else if (user.Student != null)
            {
                await _userRepository.DeleteStudentAsync(user.Student);
            }

            await _userRepository.DeleteUserAsync(user);
        }
        public async Task<UserDto> GetMyProfileAsync(int userId)
        {
            var user = await _userRepository.GetFullUserByIdAsync(userId)
                       ?? throw new ArgumentException("Пользователь не найден");

            return _mapper.Map<UserDto>(user);
        }

        public async Task UpdateMyProfileAsync(int userId, UpdateProfileRequestDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("Пользователь не найден.");

            
            var oldPhotoPath = user.Photo;

        
            if (dto.Photo != null && dto.Photo.Length > 0)
            {
          
                user.Photo = await _fileService.SaveImageAsync(dto.Photo);

           
                if (!string.IsNullOrEmpty(oldPhotoPath))
                {
                    _fileService.DeleteImage(oldPhotoPath);
                }
            }

        
            if (!string.IsNullOrWhiteSpace(dto.Email))
            {
                if (user.Email != dto.Email)
                {
                    var emailErrors = EmailValidator.Validate(dto.Email);
                    if (emailErrors.Any())
                        throw new InvalidEmailException(string.Join(" ", emailErrors));

                    bool exists = await _userRepository.EmailExistsAsync(dto.Email);
                    if (exists)
                        throw new InvalidEmailException("Такой email уже используется.");

                    user.Email = dto.Email;
                }
            }
        
            if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
            {
                if (!PhoneValidator.IsValid(dto.PhoneNumber))
                    throw new InvalidPhoneNumberException("Некорректный формат телефона.");

                user.Phone = dto.PhoneNumber;
            }

            await _userRepository.UpdateUserAsync(user);
        }
        public async Task UpdateUserByAdminAsync(int userId, UpdateUserRequestDto dto)
        {
            var user = await _userRepository.GetFullUserByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("Пользователь не найден.");

            if (!string.IsNullOrWhiteSpace(dto.Email) && user.Email != dto.Email)
            {
                var emailErrors = EmailValidator.Validate(dto.Email);
                if (emailErrors.Any())
                    throw new InvalidEmailException(string.Join(" ", emailErrors));

                if (await _userRepository.EmailExistsAsync(dto.Email))
                    throw new InvalidEmailException("Такой email уже используется.");

                user.Email = dto.Email;
            }

            if (!string.IsNullOrWhiteSpace(dto.Phone))
            {
                if (!PhoneValidator.IsValid(dto.Phone))
                    throw new InvalidPhoneNumberException("Некорректный формат телефона. Используйте формат +7XXXXXXXXXX или 8XXXXXXXXXX.");

                user.Phone = dto.Phone;
            }

            if (!string.IsNullOrWhiteSpace(dto.Surname))
                user.Surname = dto.Surname;
            if (!string.IsNullOrWhiteSpace(dto.Name))
                user.Name = dto.Name;
            if (!string.IsNullOrWhiteSpace(dto.Patronymic))
                user.Patronymic = dto.Patronymic;
            if (!string.IsNullOrWhiteSpace(dto.Login))
                user.Login = dto.Login;
            if (!string.IsNullOrWhiteSpace(dto.PhotoPath))
                user.Photo = dto.PhotoPath;

           
            if (user.Student != null)
            {
                if (dto.GroupId.HasValue)
                    user.Student.GroupId = dto.GroupId.Value;

                if (dto.CuratorId.HasValue)
                    user.Student.TeacherId = dto.CuratorId.Value;

            
                if (user.Student.GroupId == 0 || user.Student.TeacherId == 0)
                    throw new InvalidStudentDataException("Для студента должны быть указаны GroupId и TeacherId.");
            }

            await _userRepository.UpdateUserAsync(user);
        }



    }
}
