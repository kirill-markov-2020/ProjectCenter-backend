using ProjectCenter.Application.DTOs.User;

namespace ProjectCenter.Application.Interfaces
{
    public interface IUserService
    {
      
        Task<CreateUserResponseDto> CreateUserAsync(CreateUserRequestDto dto);

        Task<List<UserDto>> GetAllUsersAsync();

       
        Task DeleteUserAsync(int id, DeleteUserRequestDto? dto = null);

   
        Task<UserDto> GetMyProfileAsync(int userId);

    
        Task UpdateMyProfileAsync(int userId, UpdateProfileRequestDto dto);
        
        Task UpdateUserByAdminAsync(int userId, UpdateUserRequestDto dto);

        Task<List<UserDto>> GetActiveUsersAsync();  
        Task<List<UserDto>> GetGraduatedUsersAsync();


    }
}
