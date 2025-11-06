namespace ProjectCenter.Application.DTOs
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string StudentName { get; set; }
        public string TeacherName { get; set; }
        public string StatusName { get; set; }
        public string TypeName { get; set; }
        public string SubjectName { get; set; }
        public bool IsPublic { get; set; }
        public DateTime DateDeadline { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<CommentDto> Comments { get; set; } = new();
    }
}
