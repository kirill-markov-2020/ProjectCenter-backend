using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectCenter.Application.DTOs;
using ProjectCenter.Application.DTOs.UpdateProject;
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
        private readonly IFileService _fileService;

        public ProjectService(IProjectRepository projectRepository, IUserRepository userRepository, IMapper mapper, IFileService fileService)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _fileService = fileService;
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
        public async Task<ProjectDto> UpdateProjectAsync(int projectId, UpdateProjectRequestDto dto)
        {
            var project = await _projectRepository.GetProjectByIdAsync(projectId);

            if (project == null)
                throw new ProjectNotFoundException(projectId);

            // Обновляем только те поля, которые переданы в DTO
            if (!string.IsNullOrWhiteSpace(dto.Title))
                project.Title = dto.Title;

            if (dto.TeacherId.HasValue)
                project.TeacherId = dto.TeacherId.Value;

            if (dto.StatusId.HasValue)
                project.StatusId = dto.StatusId.Value;

            if (dto.TypeId.HasValue)
                project.TypeId = dto.TypeId.Value;

            if (dto.SubjectId.HasValue)
                project.SubjectId = dto.SubjectId.Value;

            if (!string.IsNullOrWhiteSpace(dto.FileProject))
                project.FileProject = dto.FileProject;

            if (!string.IsNullOrWhiteSpace(dto.FileDocumentation))
                project.FileDocumentation = dto.FileDocumentation;

            if (dto.IsPublic.HasValue)
                project.IsPublic = dto.IsPublic.Value;

            if (dto.DateDeadline.HasValue)
                project.DateDeadline = dto.DateDeadline.Value;

            await _projectRepository.UpdateProjectAsync(project);

            // Возвращаем обновленный проект
            var updatedProject = await _projectRepository.GetProjectByIdAsync(projectId);
            return _mapper.Map<ProjectDto>(updatedProject);
        }
        public async Task<ProjectDto> UpdateStudentProjectAsync(int projectId, UpdateStudentProjectRequestDto dto, int studentUserId)
        {
            var project = await _projectRepository.GetProjectByIdAsync(projectId);

            if (project == null)
                throw new ProjectNotFoundException(projectId);

            // Проверяем, что проект принадлежит студенту
            var student = await _userRepository.GetStudentByUserIdAsync(studentUserId);
            if (student == null || project.StudentId != student.Id)
                throw new ProjectAccessDeniedException();

            // Обновляем файл проекта (архив)
            if (dto.NewProjectFile != null)
            {
                // Удаляем старый файл если есть
                if (!string.IsNullOrEmpty(project.FileProject))
                    _fileService.DeleteProjectFile(project.FileProject);

                project.FileProject = await _fileService.SaveProjectFileAsync(dto.NewProjectFile);
            }
            else if (dto.RemoveProjectFile == true && !string.IsNullOrEmpty(project.FileProject))
            {
                _fileService.DeleteProjectFile(project.FileProject);
                project.FileProject = null;
            }

            // Обновляем файл документации (текстовый)
            if (dto.NewDocumentationFile != null)
            {
                // Удаляем старый файл если есть
                if (!string.IsNullOrEmpty(project.FileDocumentation))
                    _fileService.DeleteDocumentationFile(project.FileDocumentation);

                project.FileDocumentation = await _fileService.SaveDocumentationFileAsync(dto.NewDocumentationFile);
            }
            else if (dto.RemoveDocumentationFile == true && !string.IsNullOrEmpty(project.FileDocumentation))
            {
                _fileService.DeleteDocumentationFile(project.FileDocumentation);
                project.FileDocumentation = null;
            }

            // Обновляем видимость
            if (dto.IsPublic.HasValue)
                project.IsPublic = dto.IsPublic.Value;

            await _projectRepository.UpdateProjectAsync(project);

            // Возвращаем обновленный проект
            var updatedProject = await _projectRepository.GetProjectByIdAsync(projectId);
            return _mapper.Map<ProjectDto>(updatedProject);
        }
        public async Task DeleteProjectAsync(int projectId)
        {
            var project = await _projectRepository.GetProjectByIdAsync(projectId);
            if (project == null)
                throw new ArgumentException("Проект не найден");

            // 1. Удаляем файл проекта
            if (!string.IsNullOrEmpty(project.FileProject))
                _fileService.DeleteProjectFile(project.FileProject);

            // 2. Удаляем файл документации
            if (!string.IsNullOrEmpty(project.FileDocumentation))
                _fileService.DeleteDocumentationFile(project.FileDocumentation);

            // 3. Удаляем комментарии
            if (project.Comments != null && project.Comments.Any())
                project.Comments.Clear(); // EF удалит каскадно

            // 4. Удаляем сам проект
            await _projectRepository.DeleteProjectAsync(project);
        }
        public async Task<ProjectDto?> GetMyProjectAsync(int studentUserId)
        {
            // 1. Получаем студента по UserId
            var student = await _userRepository.GetStudentByUserIdAsync(studentUserId);
            if (student == null)
            {
                // Если пользователь не является студентом, возвращаем null
                // В контроллере это обработается как 404
                return null;
            }

            // 2. Ищем активный проект студента
            var activeProject = await _projectRepository.GetActiveProjectByStudentIdAsync(student.Id);
            if (activeProject == null)
            {
                return null;
            }

            // 3. Загружаем полные данные проекта (с включениями)
            var fullProject = await _projectRepository.GetProjectByIdAsync(activeProject.Id);

            // 4. Маппим в DTO и возвращаем
            return _mapper.Map<ProjectDto>(fullProject);
        }



    }
}
