using ProjectCenter.Application.DTOs;
using ProjectCenter.Application.DTOs.CreateUser;

namespace ProjectCenter.Application.Interfaces
{
    public interface IUserService
    {
        Task<CreateUserResponseDto> CreateUserAsync(CreateUserRequestDto dto);
    }
}
