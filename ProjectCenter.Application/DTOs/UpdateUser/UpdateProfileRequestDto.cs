using Microsoft.AspNetCore.Http;

namespace ProjectCenter.Application.DTOs.UpdateUser
{
    public class UpdateProfileRequestDto
    {
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public IFormFile? Photo { get; set; }  
    }
}