namespace ProjectCenter.Application.DTOs.Auth
{
    public class ResetPasswordResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string? ResetToken { get; set; } 
    }
}