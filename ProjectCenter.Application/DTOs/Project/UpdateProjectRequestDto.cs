namespace ProjectCenter.Application.DTOs.Project
{
    public class UpdateProjectRequestDto
    {
        public string? Title { get; set; }
        public int? TeacherId { get; set; }
        public int? StatusId { get; set; }
        public int? TypeId { get; set; }
        public int? SubjectId { get; set; }
        public bool? IsPublic { get; set; }
        public DateTime? DateDeadline { get; set; }
    }
}