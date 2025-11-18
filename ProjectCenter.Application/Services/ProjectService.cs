using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectCenter.Application.DTOs;
using ProjectCenter.Application.Interfaces;
using ProjectCenter.Core.Entities;
using ProjectCenter.Core.Exceptions;

namespace ProjectCenter.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository; 
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository projectRepository, IUserRepository userRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository; 
            _mapper = mapper;
        }

        public async Task<List<ProjectDto>> GetProjectsForUserAsync(int userId, bool isAdmin)
        {
            var projects = isAdmin
                ? await _projectRepository.GetAllProjectsAsync()
                : await _projectRepository.GetPublicProjectsAsync();

            return _mapper.Map<List<ProjectDto>>(projects);
        }
        public async Task<ProjectDto> GetProjectByIdAsync(int id)
        {
            var project = await _projectRepository.GetProjectByIdAsync(id);

            if (project == null)
                throw new ProjectNotFoundException(id);

            return _mapper.Map<ProjectDto>(project);
        }
        public async Task<ProjectDto> CreateProjectAsync(CreateProjectRequestDto dto, int studentUserId)
        {
            // Находим студента с его куратором
            var student = await _userRepository.GetStudentByUserIdAsync(studentUserId)
                ?? throw new StudentNotFoundException(studentUserId);

            // Проверяем, что у студента есть куратор
            if (student.TeacherId == 0 || student.Teacher == null)
                throw new InvalidOperationException("У студента не назначен куратор. Обратитесь к администратору.");

            // Создаем проект с куратором студента
            var project = new Project
            {
                Title = dto.Title,
                StudentId = student.Id,
                TeacherId = student.TeacherId, // Берем куратора студента!
                TypeId = dto.TypeId,
                SubjectId = dto.SubjectId,
                StatusId = 1, // Статус "Черновик" или "В работе"
                IsPublic = dto.IsPublic,
                FileProject = null,
                FileDocumentation = null,
                DateDeadline = new DateTime(DateTime.Now.Year + 1, 6, 30), // 30 июня следующего года
                CreatedDate = DateTime.UtcNow
            };

            await _projectRepository.AddProjectAsync(project);

            // Загружаем созданный проект со всеми связями для маппинга
            var createdProject = await _projectRepository.GetProjectByIdAsync(project.Id);
            return _mapper.Map<ProjectDto>(createdProject);
        }
    }
}
