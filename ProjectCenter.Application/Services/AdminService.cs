using ProjectCenter.Application.DTOs.Admin;
using ProjectCenter.Application.Interfaces;

namespace ProjectCenter.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IProjectRepository _projectRepo;
        private readonly IDirectoryRepository _directoryRepo;
        private readonly IUserRepository _userRepo;
        private readonly ITeacherRepository _teacherRepo;
        private readonly IGroupRepository _groupRepo;

        public AdminService(
            IProjectRepository projectRepo,
            IDirectoryRepository directoryRepo,
            IUserRepository userRepo,
            ITeacherRepository teacherRepo,
            IGroupRepository groupRepo)
        {
            _projectRepo = projectRepo;
            _directoryRepo = directoryRepo;
            _userRepo = userRepo;
            _teacherRepo = teacherRepo;
            _groupRepo = groupRepo;
        }

        public async Task<AdminDashboardResponse> GetDashboardAsync()
        {
            var projects = await _projectRepo.GetAllProjectsAsync();
            var statuses = await _directoryRepo.GetStatusesAsync();
            var types = await _directoryRepo.GetTypesAsync();
            var groups = await _groupRepo.GetAllAsync();
            var students = await _userRepo.GetAllStudentsAsync();
            var teachers = await _teacherRepo.GetAllTeachersAsync();
            var subjects = await _directoryRepo.GetSubjectsAsync();

            var now = DateTime.Now;

            return new AdminDashboardResponse
            {
                TotalProjects = projects.Count,
                TotalStudents = students.Count,
                TotalTeachers = teachers.Count,
                TotalGroups = groups.Count,
                TotalSubjects = subjects.Count,
                OverdueProjects = projects.Count(p => p.DateDeadline < now),
                ProjectsWithoutGrade = projects.Count(p => p.Grade == null),
                AverageGrade = projects.Where(p => p.Grade != null)
                    .Select(p => p.Grade.Value)
                    .DefaultIfEmpty()
                    .Average(),

                ProjectsByStatus = statuses.Select(s => new StatusCountDto
                {
                    StatusName = s.Name,
                    Count = projects.Count(p => p.StatusId == s.Id)
                }).ToList(),

                ProjectsByType = types.Select(t => new TypeCountDto
                {
                    TypeName = t.Name,
                    Count = projects.Count(p => p.TypeId == t.Id)
                }).ToList(),

                ProjectsByGroup = groups.Select(g => new GroupStatsDto
                {
                    GroupName = g.Name,
                    TotalProjects = projects.Count(p => p.Student.GroupId == g.Id),
                    StudentsCount = g.Students.Count
                }).ToList()
            };
        }

        public async Task<List<LastProjectDto>> GetLastProjectsAsync(int count)
        {
            var projects = await _projectRepo.GetAllProjectsAsync();

            return projects
                .OrderByDescending(p => p.CreatedDate)
                .Take(count)
                .Select(p => new LastProjectDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    StudentName = $"{p.Student.User.Surname} {p.Student.User.Name}",
                    StatusName = p.Status.Name,
                    CreatedDate = p.CreatedDate
                })
                .ToList();
        }

        public async Task<List<ActiveTeacherDto>> GetActiveTeachersAsync()
        {
            var teachers = await _teacherRepo.GetAllTeachersAsync();
            var projects = await _projectRepo.GetAllProjectsAsync();

            return teachers
                .Select(t => new ActiveTeacherDto
                {
                    TeacherName = $"{t.User.Surname} {t.User.Name}",
                    ProjectCount = projects.Count(p => p.TeacherId == t.Id)
                })
                .OrderByDescending(t => t.ProjectCount)
                .ToList();
        }

        public async Task<List<RecentActivityDto>> GetRecentActivityAsync(int count)
        {
            var projects = await _projectRepo.GetAllProjectsAsync();

            var comments = projects
                .Where(p => p.Comments != null)
                .SelectMany(p => p.Comments.Select(c => new RecentActivityDto
                {
                    Type = "comment",
                    Description = c.Text,
                    UserName = $"{c.User.Surname} {c.User.Name}",
                    ProjectTitle = p.Title,
                    Date = c.Date
                }));

            var grades = projects
                .Where(p => p.Grade != null)
                .Select(p => new RecentActivityDto
                {
                    Type = "grade",
                    Description = $"Оценка: {p.Grade.Value}",
                    UserName = $"{p.Grade.Teacher.User.Surname} {p.Grade.Teacher.User.Name}",
                    ProjectTitle = p.Title,
                    Date = p.Grade.CreatedAt
                });

            return comments
                .Concat(grades)
                .OrderByDescending(a => a.Date)
                .Take(count)
                .ToList();
        }
    }
}
