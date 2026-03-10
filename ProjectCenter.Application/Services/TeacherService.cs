using ProjectCenter.Application.DTOs.Directory;

using ProjectCenter.Application.Interfaces;

namespace ProjectCenter.Application.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _repo;

        public TeacherService(ITeacherRepository repo)
        {
            _repo = repo;
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
    }
}
