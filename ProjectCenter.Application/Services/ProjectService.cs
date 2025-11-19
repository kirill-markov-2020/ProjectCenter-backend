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
         
            var student = await _userRepository.GetStudentByUserIdAsync(studentUserId)
                ?? throw new StudentNotFoundException(studentUserId);

            if (student.TeacherId == 0 || student.Teacher == null)
                throw new InvalidOperationException("У студента не назначен куратор. Обратитесь к администратору.");

       
            var activeProject = await _projectRepository.GetActiveProjectByStudentIdAsync(student.Id);
            if (activeProject != null)
                throw new ActiveProjectExistsException(activeProject.Title);

            var project = new Project
            {
                Title = dto.Title,
                StudentId = student.Id,
                TeacherId = student.TeacherId,
                TypeId = dto.TypeId,
                SubjectId = dto.SubjectId,
                StatusId = 1, 
                IsPublic = dto.IsPublic,
                FileProject = null,
                FileDocumentation = null,
                DateDeadline = new DateTime(DateTime.Now.Year + 1, 6, 30),
                CreatedDate = DateTime.UtcNow
            };

            await _projectRepository.AddProjectAsync(project);

            var createdProject = await _projectRepository.GetProjectByIdAsync(project.Id);
            return _mapper.Map<ProjectDto>(createdProject);
        }
    }
}
