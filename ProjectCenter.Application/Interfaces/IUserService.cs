using ProjectCenter.Application.DTOs;
using ProjectCenter.Application.DTOs.CreateUser;
using ProjectCenter.Application.DTOs.UpdateUser;

namespace ProjectCenter.Application.Interfaces
{
    public interface IUserService
    {
        Task<CreateUserResponseDto> CreateUserAsync(CreateUserRequestDto dto);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UpdateUserResponseDto> UpdateUserAsync(int id, UpdateUserRequestDto dto, string currentUserRole, int currentUserId);

        Task DeleteUserAsync(int id);


    }
}
