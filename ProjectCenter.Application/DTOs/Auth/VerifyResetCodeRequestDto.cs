namespace ProjectCenter.Application.DTOs.Auth
{
    public class VerifyResetCodeRequestDto
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}