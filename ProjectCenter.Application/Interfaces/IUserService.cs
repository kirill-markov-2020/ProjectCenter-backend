using ProjectCenter.Application.DTOs;
using ProjectCenter.Application.DTOs.CreateUser;

namespace ProjectCenter.Application.Interfaces
{
    public interface IUserService
    {
        // Создать пользователя (Admin endpoint)
        Task<CreateUserResponseDto> CreateUserAsync(CreateUserRequestDto dto);

        // Получить список всех пользователей (Admin endpoint)
        Task<List<UserDto>> GetAllUsersAsync();

        // Удалить пользователя по id (Admin endpoint)
        Task DeleteUserAsync(int id);

        // Новый: получить профиль текущего пользователя (использует userId из контекста)
        Task<UserDto> GetMyProfileAsync(int userId);

        // (Позже) Обновление профиля — добавлять только когда будешь готов:
        // Task UpdateMyProfileAsync(int userId, UpdateUserRequestDto dto);
    }
}
