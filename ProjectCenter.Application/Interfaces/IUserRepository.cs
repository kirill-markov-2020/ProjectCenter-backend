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
        Task<User?> GetByIdAsync(int id);

        
        Task<User?> GetFullUserByIdAsync(int id);

       
        Task DeleteUserAsync(User user);
        Task DeleteStudentAsync(Student student);
        Task DeleteTeacherAsync(Teacher teacher);
        Task UpdateUserAsync(User user);
        


    }
}
