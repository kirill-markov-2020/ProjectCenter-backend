namespace ProjectCenter.Application.DTOs
{
    public class UpdateProjectRequestDto
    {
        public string? Title { get; set; }
        public int? TeacherId { get; set; }
        public int? StatusId { get; set; }
        public int? TypeId { get; set; }
        public int? SubjectId { get; set; }
        public string? FileProject { get; set; }
        public string? FileDocumentation { get; set; }
        public bool? IsPublic { get; set; }
        public DateTime? DateDeadline { get; set; }
    }
}