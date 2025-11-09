namespace ProjectCenter.Application.DTOs.CreateUser
{
    public class CreateUserRequestDto
    {
        // Роль: "Admin", "Teacher", "Student"
        public string Role { get; set; }

        // User fields (Photo необязательное)
        public string Surname { get; set; }
        public string Name { get; set; }
        public string? Patronymic { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string? Photo { get; set; } // необязательно

        // Для студента: обязательны GroupId и TeacherId (куратор)
        public int? GroupId { get; set; }
        public int? TeacherId { get; set; }
    }
}
