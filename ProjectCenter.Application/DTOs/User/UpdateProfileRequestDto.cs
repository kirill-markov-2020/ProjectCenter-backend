using Microsoft.AspNetCore.Http;

namespace ProjectCenter.Application.DTOs.User
{
    public class UpdateProfileRequestDto
    {
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public IFormFile? Photo { get; set; }  
    }
}