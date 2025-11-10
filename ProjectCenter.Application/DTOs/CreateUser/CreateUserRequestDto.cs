namespace ProjectCenter.Application.DTOs.CreateUser
{
    public class CreateUserRequestDto
    {
        public string Role { get; set; }

        public string Surname { get; set; }
        public string Name { get; set; }
        public string? Patronymic { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string? Photo { get; set; } 

        public int? GroupId { get; set; }
        public int? TeacherId { get; set; }
    }
}
