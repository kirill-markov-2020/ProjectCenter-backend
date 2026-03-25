using ProjectCenter.Application.DTOs;
using ProjectCenter.Application.DTOs.Directory;

using ProjectCenter.Application.Interfaces;
using ProjectCenter.Core.Exceptions;

namespace ProjectCenter.Application.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _repo;
        private readonly IUserRepository _userRepository;

        public TeacherService(ITeacherRepository repo, IUserRepository userRepository)
        {
            _repo = repo;
            _userRepository = userRepository;
        }

        public async Task<List<TeacherDto>> GetAllTeachersAsync()
        {
            var teachers = await _repo.GetAllTeachersAsync();

            return teachers.Select(t => new TeacherDto
            {
                Id = t.Id,
                Surname = t.User.Surname,
                Name = t.User.Name,
                Patronymic = t.User.Patronymic
            }).ToList();
        }
        public async Task<List<StudentShortDto>> GetMyStudentsAsync(int userId)
        {
            var user = await _userRepository.GetFullUserByIdAsync(userId);

            if (user == null)
                throw new UserNotFoundException(userId);

            if (user.Teacher == null)
                throw new AccessDeniedException("Только преподаватель может просматривать список студентов.");

            var students = await _repo.GetStudentsByTeacherIdAsync(user.Teacher.Id);

            if (!students.Any())
                throw new ArgumentException("У вас пока нет закреплённых студентов.");

            return students.Select(s => new StudentShortDto
            {
                Id = s.Id,
                FullName = $"{s.User.Surname} {s.User.Name} {s.User.Patronymic}".Trim(),
                GroupName = s.Group?.Name
            }).ToList();
        }
    }
}
