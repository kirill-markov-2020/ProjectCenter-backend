using ProjectCenter.Core.Entities;

namespace ProjectCenter.Application.Interfaces
{
    public interface ITeacherRepository
    {
        Task<List<Teacher>> GetAllTeachersAsync();
    }
}
