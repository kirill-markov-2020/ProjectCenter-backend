namespace ProjectCenter.Application.DTOs.UpdateUser
{
    public class UpdateUserResponseDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = default!;
        public string Role { get; set; } = default!;
        public string Login { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string? Photo { get; set; }
    }
}
