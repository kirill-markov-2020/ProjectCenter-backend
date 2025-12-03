namespace ProjectCenter.Application.DTOs.UpdateUser
{
    public class UpdateUserRequestDto
    {
        public string? Surname { get; set; }
        public string? Name { get; set; }
        public string? Patronymic { get; set; }

        public string? Login { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public string? PhotoPath { get; set; }

        public int? GroupId { get; set; }
        public int? CuratorId { get; set; }
    }
}
