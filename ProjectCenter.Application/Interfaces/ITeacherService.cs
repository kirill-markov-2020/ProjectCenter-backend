using ProjectCenter.Application.DTOs;
using ProjectCenter.Application.DTOs.Directory;

namespace ProjectCenter.Application.Interfaces
{
    public interface ITeacherService
    {
        Task<List<TeacherDto>> GetAllTeachersAsync();
        Task<List<StudentShortDto>> GetMyStudentsAsync(int userId);
    }
}
