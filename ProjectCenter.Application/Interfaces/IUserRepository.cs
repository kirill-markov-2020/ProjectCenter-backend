using ProjectCenter.Core.Entities;

namespace ProjectCenter.Application.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task AddTeacherAsync(Teacher teacher);
        Task AddStudentAsync(Student student);
        Task<bool> LoginExistsAsync(string login);
        Task<bool> EmailExistsAsync(string email);
        Task<List<User>> GetAllAsync();
    }
}
