namespace ProjectCenter.Application.DTOs.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string? Patronymic { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public string? Photo { get; set; }
        public string? GroupDisplayName { get; set; }
        public string? CuratorName { get; set; }
        public DateTime? DateEnrolled { get; set; }
        public DateTime? DateGraduated { get; set; }
    }
}
