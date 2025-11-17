using ProjectCenter.Core.Entities;

namespace ProjectCenter.Application.Interfaces
{
    public interface IUserRepository
    {
        // Создание
        Task AddUserAsync(User user);
        Task AddTeacherAsync(Teacher teacher);
        Task AddStudentAsync(Student student);

        // Проверки уникальности
        Task<bool> LoginExistsAsync(string login);
        Task<bool> EmailExistsAsync(string email);

        // Получение
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);

        // Новый: получить пользователя со всеми нужными навигациями (group, teacher, students и т.д.)
        Task<User?> GetFullUserByIdAsync(int id);

        // Удаление
        Task DeleteUserAsync(User user);
        Task DeleteStudentAsync(Student student);
        Task DeleteTeacherAsync(Teacher teacher);
    }
}
