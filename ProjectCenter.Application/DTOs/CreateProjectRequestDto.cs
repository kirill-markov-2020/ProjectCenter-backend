public class CreateProjectRequestDto
{
    public string Title { get; set; }
    public int TypeId { get; set; }
    public int SubjectId { get; set; }
    public bool IsPublic { get; set; }
}