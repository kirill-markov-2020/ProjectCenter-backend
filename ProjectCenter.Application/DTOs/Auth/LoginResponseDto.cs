namespace ProjectCenter.Application.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
    }
}
