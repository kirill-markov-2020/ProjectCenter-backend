using ProjectCenter.Application.DTOs;
using ProjectCenter.Application.DTOs.CreateUser;
using ProjectCenter.Application.DTOs.UpdateUser;

namespace ProjectCenter.Application.Interfaces
{
    public interface IUserService
    {
      
        Task<CreateUserResponseDto> CreateUserAsync(CreateUserRequestDto dto);

        Task<List<UserDto>> GetAllUsersAsync();

       
        Task DeleteUserAsync(int id);

   
        Task<UserDto> GetMyProfileAsync(int userId);

    
        Task UpdateMyProfileAsync(int userId, UpdateProfileRequestDto dto);
        
        Task UpdateUserByAdminAsync(int userId, UpdateUserRequestDto dto);
            // ... остальное уже есть
        


    }
}
