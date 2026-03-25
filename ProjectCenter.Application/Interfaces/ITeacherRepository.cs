using ProjectCenter.Core.Entities;

namespace ProjectCenter.Application.Interfaces
{
    public interface ITeacherRepository
    {
        Task<List<Teacher>> GetAllTeachersAsync();
        Task<List<Student>> GetStudentsByTeacherIdAsync(int teacherId);
    }
}
