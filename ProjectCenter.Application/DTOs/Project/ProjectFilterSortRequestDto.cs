using ProjectCenter.Core.Enums;

namespace ProjectCenter.Application.DTOs.Project
{
    public class ProjectFilterSortRequestDto
    {
        public ProjectSortBy? SortBy { get; set; } = ProjectSortBy.CreatedDateDesc; 

        public int? StatusId { get; set; }
        public int? TypeId { get; set; }
        public int? SubjectId { get; set; }
        public bool? IsPublic { get; set; }
        public string? SearchTerm { get; set; }

   
    }
}