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


        // ProjectCenter.Application/Services/ProjectService.cs
        public async Task<List<ProjectDto>> GetProjectsForUserAsync(int userId)
        {
            var user = await _userRepository.GetFullUserByIdAsync(userId);

            // Админ — все проекты
            if (user.IsAdmin)
            {
                var allProjects = await _projectRepository.GetAllProjectsAsync();
                return _mapper.Map<List<ProjectDto>>(allProjects);
            }

            // Учитель — все проекты (тоже)
            if (user.Teacher != null)
            {
                var allProjects = await _projectRepository.GetAllProjectsAsync();
                return _mapper.Map<List<ProjectDto>>(allProjects);
            }

            // Студент — только публичные
            var publicProjects = await _projectRepository.GetPublicProjectsAsync();
            return _mapper.Map<List<ProjectDto>>(publicProjects);
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

            var updatedProject = await _projectRepository.GetProjectByIdAsync(projectId);
            return _mapper.Map<ProjectDto>(updatedProject);
        }
        public async Task<ProjectDto> UpdateStudentProjectAsync(int projectId, UpdateStudentProjectRequestDto dto, int studentUserId)
        {
            var project = await _projectRepository.GetProjectByIdAsync(projectId);

            if (project == null)
                throw new ProjectNotFoundException(projectId);

          
            var student = await _userRepository.GetStudentByUserIdAsync(studentUserId);
            if (student == null || project.StudentId != student.Id)
                throw new ProjectAccessDeniedException();

         
            if (dto.NewProjectFile != null)
            {
  
                if (!string.IsNullOrEmpty(project.FileProject))
                    _fileService.DeleteProjectFile(project.FileProject);

                project.FileProject = await _fileService.SaveProjectFileAsync(dto.NewProjectFile);
            }
            else if (dto.RemoveProjectFile == true && !string.IsNullOrEmpty(project.FileProject))
            {
                _fileService.DeleteProjectFile(project.FileProject);
                project.FileProject = null;
            }

    
            if (dto.NewDocumentationFile != null)
            {
  
                if (!string.IsNullOrEmpty(project.FileDocumentation))
                    _fileService.DeleteDocumentationFile(project.FileDocumentation);

                project.FileDocumentation = await _fileService.SaveDocumentationFileAsync(dto.NewDocumentationFile);
            }
            else if (dto.RemoveDocumentationFile == true && !string.IsNullOrEmpty(project.FileDocumentation))
            {
                _fileService.DeleteDocumentationFile(project.FileDocumentation);
                project.FileDocumentation = null;
            }


            if (dto.IsPublic.HasValue)
                project.IsPublic = dto.IsPublic.Value;

            await _projectRepository.UpdateProjectAsync(project);


            var updatedProject = await _projectRepository.GetProjectByIdAsync(projectId);
            return _mapper.Map<ProjectDto>(updatedProject);
        }
        public async Task DeleteProjectAsync(int projectId)
        {
            var project = await _projectRepository.GetProjectByIdAsync(projectId);
            if (project == null)
                throw new ArgumentException("Проект не найден");

        
            if (!string.IsNullOrEmpty(project.FileProject))
                _fileService.DeleteProjectFile(project.FileProject);

     
            if (!string.IsNullOrEmpty(project.FileDocumentation))
                _fileService.DeleteDocumentationFile(project.FileDocumentation);


            if (project.Comments != null && project.Comments.Any())
                project.Comments.Clear(); 

    
            await _projectRepository.DeleteProjectAsync(project);
        }
        public async Task<ProjectDto?> GetMyProjectAsync(int studentUserId)
        {
          
            var student = await _userRepository.GetStudentByUserIdAsync(studentUserId);
            if (student == null)
            {
      
                return null;
            }

       
            var activeProject = await _projectRepository.GetActiveProjectByStudentIdAsync(student.Id);
            if (activeProject == null)
            {
                return null;
            }

            var fullProject = await _projectRepository.GetProjectByIdAsync(activeProject.Id);

       
            return _mapper.Map<ProjectDto>(fullProject);
        }
        public async Task AddCommentAsync(int projectId, int userId, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Комментарий не может быть пустым.");

    
            var user = await _userRepository.GetFullUserByIdAsync(userId);
            if (user == null)
                throw new UserNotFoundException(userId);


            if (user.Teacher == null)
                throw new AccessDeniedException("Только преподаватель может оставлять комментарии.");

    
            var project = await _projectRepository.GetProjectByIdAsync(projectId);
            if (project == null)
                throw new ProjectNotFoundException(projectId);

    
            if (project.TeacherId != user.Teacher.Id)
                throw new AccessDeniedException("Вы можете комментировать только проекты своих студентов.");

            var comment = new Comment
            {
                Text = text,
                Date = DateTime.UtcNow,
                UserId = user.Id,
                ProjectId = project.Id
            };

   
            project.Comments.Add(comment);


            await _projectRepository.UpdateProjectAsync(project);
        }
       
        public async Task<ProjectDto> GetTeacherStudentProjectAsync(int projectId, int teacherUserId)
        {
 
            var teacher = await _userRepository.GetFullUserByIdAsync(teacherUserId);
            if (teacher?.Teacher == null)
                throw new AccessDeniedException("Вы не являетесь преподавателем");

          
            var project = await _projectRepository.GetProjectByIdAndTeacherIdAsync(projectId, teacher.Teacher.Id);

            if (project == null)
                throw new NoCuratorProjectException(projectId);

   
            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<ProjectDto> UpdateProjectByTeacherAsync(int projectId, UpdateTeacherProjectRequestDto dto, int teacherUserId)
        {
            var teacher = await _userRepository.GetFullUserByIdAsync(teacherUserId);
            if (teacher?.Teacher == null)
                throw new AccessDeniedException("Вы не являетесь преподавателем");
            var project = await _projectRepository.GetProjectByIdAsync(projectId);
            if (project == null)
                throw new ProjectNotFoundException(projectId);
            if (project.TeacherId != teacher.Teacher.Id)
                throw new AccessDeniedException("Вы можете редактировать только проекты своих студентов.");
            if (!string.IsNullOrWhiteSpace(dto.Title))
                project.Title = dto.Title;
            if (dto.TypeId.HasValue)
                project.TypeId = dto.TypeId.Value;
            if (dto.SubjectId.HasValue)
                project.SubjectId = dto.SubjectId.Value;
            if (dto.DateDeadline.HasValue)
                project.DateDeadline = dto.DateDeadline.Value;
            await _projectRepository.UpdateProjectAsync(project);
            var updatedProject = await _projectRepository.GetProjectByIdAsync(projectId);
            return _mapper.Map<ProjectDto>(updatedProject);
        }



    }
}
