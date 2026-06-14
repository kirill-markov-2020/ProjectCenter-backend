namespace ProjectCenter.Application.DTOs.Admin
{
    public class AdminDashboardResponse
    {
        public int TotalProjects { get; set; }
        public int TotalStudents { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalGroups { get; set; }
        public int TotalSubjects { get; set; }
        public int OverdueProjects { get; set; }
        public int ProjectsWithoutGrade { get; set; }
        public double AverageGrade { get; set; }
        public List<StatusCountDto> ProjectsByStatus { get; set; }
        public List<TypeCountDto> ProjectsByType { get; set; }
        public List<GroupStatsDto> ProjectsByGroup { get; set; }
    }

    public class StatusCountDto
    {
        public string StatusName { get; set; }
        public int Count { get; set; }
    }

    public class TypeCountDto
    {
        public string TypeName { get; set; }
        public int Count { get; set; }
    }

    public class GroupStatsDto
    {
        public string GroupName { get; set; }
        public int TotalProjects { get; set; }
        public int StudentsCount { get; set; }
    }

    public class LastProjectDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string StudentName { get; set; }
        public string StatusName { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class ActiveTeacherDto
    {
        public string TeacherName { get; set; }
        public int ProjectCount { get; set; }
    }

    public class RecentActivityDto
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public string ProjectTitle { get; set; }
        public DateTime Date { get; set; }
    }
}
