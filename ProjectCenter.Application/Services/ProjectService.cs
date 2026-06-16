using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectCenter.Application.DTOs;
using ProjectCenter.Application.DTOs.Directory;
using ProjectCenter.Application.DTOs.Project;
using ProjectCenter.Application.Interfaces;
using ProjectCenter.Core.Entities;
using ProjectCenter.Core.Enums;
using ProjectCenter.Core.Exceptions;

namespace ProjectCenter.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository; 
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly INotificationService _notificationService;
        private readonly IDirectoryRepository _directoryRepository;
        private readonly IGroupRepository _groupRepository;

        public ProjectService(IProjectRepository projectRepository, IUserRepository userRepository, IMapper mapper, IFileService fileService, INotificationService notificationService, IDirectoryRepository directoryRepository, IGroupRepository groupRepository)
        {
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _fileService = fileService;
            _notificationService = notificationService;
            _directoryRepository = directoryRepository;
            _groupRepository = groupRepository;
        }


        public async Task<List<ProjectDto>> GetProjectsForUserAsync(
             int userId,
             string? searchTerm = null,
             int? year = null,
             int? groupId = null,
             ProjectSortBy? sortBy = null)
        {
            var user = await _userRepository.GetFullUserByIdAsync(userId);
            var effectiveSortBy = sortBy ?? ProjectSortBy.CreatedDateDesc;

            if (user.IsAdmin || user.Teacher != null)
            {
                var projects = await _projectRepository.GetProjectsFilteredAsync(
                    searchTerm, year, groupId, effectiveSortBy);
                return _mapper.Map<List<ProjectDto>>(projects);
            }

            var publicProjects = await _projectRepository.GetPublicProjectsFilteredAsync(
                searchTerm, year, groupId, effectiveSortBy);
            return _mapper.Map<List<ProjectDto>>(publicProjects);
        }
        public async Task<List<GroupDto>> GetAvailableGroupsForYearAsync(int year)
        {
            var projects = await _projectRepository.GetProjectsByYearAsync(year);

            var groupIds = projects
                .Where(p => p.Student?.GroupId != null)
                .Select(p => p.Student.GroupId)
                .Distinct()
                .ToList();

            if (!groupIds.Any())
                return new List<GroupDto>();

            var groups = await _groupRepository.GetByIdsAsync(groupIds);
            return _mapper.Map<List<GroupDto>>(groups);
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
            var deadline = CalculateDeadline();

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
                DateDeadline = deadline,
                CreatedDate = DateTime.Now,
                Year = deadline.Year
            };

            await _projectRepository.AddProjectAsync(project);
            var studentUser = await _userRepository.GetByIdAsync(studentUserId);
            var studentFullName = $"{studentUser.Surname} {studentUser.Name} {studentUser.Patronymic}".Trim();
            var curatorUserId = student.Teacher.UserId;
            var allUsers = await _userRepository.GetAllAsync();
            var adminUserIds = allUsers
                .Where(u => u.IsAdmin)
                .Select(u => u.Id)
                .ToList();
            await _notificationService.SendAddNewProjectNotificationAsync
                (
                    curatorUserId,
                    studentFullName,
                    project.Title,
                    adminUserIds
                );
            var createdProject = await _projectRepository.GetProjectByIdAsync(project.Id);
            return _mapper.Map<ProjectDto>(createdProject);
        }
        private DateTime CalculateDeadline()
        {
            var now = DateTime.Now;
            var currentYear = now.Year;
            var currentMonth = now.Month;

            if (currentMonth >= 9)
            {
                return new DateTime(currentYear + 1, 6, 25);
            }
            else if (currentMonth >= 1 && currentMonth <= 6)
            {
                return new DateTime(currentYear, 6, 25);
            }
            else
            {
                return new DateTime(currentYear + 1, 6, 25);
            }
        }
        public async Task<ProjectDto> CreateProjectByAdminAsync(AdminCreateProjectRequestDto dto)
        {
            var student = await _userRepository.GetStudentByUserIdAsync(dto.StudentUserId)
                ?? throw new StudentNotFoundException(dto.StudentUserId);

            if (student.TeacherId == 0 || student.Teacher == null)
                throw new InvalidOperationException($"У студента (UserId: {dto.StudentUserId}) не назначен куратор.");

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
                CreatedDate = dto.CreatedDate,
                DateDeadline = dto.DateDeadline,   
                Year = dto.DateDeadline.Year       
            };

            await _projectRepository.AddProjectAsync(project);

            var studentUser = await _userRepository.GetByIdAsync(dto.StudentUserId);
            var studentFullName = $"{studentUser.Surname} {studentUser.Name} {studentUser.Patronymic}".Trim();
            var curatorUserId = student.Teacher.UserId;
            var allUsers = await _userRepository.GetAllAsync();
            var adminUserIds = allUsers.Where(u => u.IsAdmin).Select(u => u.Id).ToList();

            await _notificationService.SendAddNewProjectNotificationAsync(curatorUserId, studentFullName, project.Title, adminUserIds);

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

            if (dto.TeacherId.HasValue && dto.TeacherId.Value != project.TeacherId)
            {
                var oldTeacherId = project.TeacherId;
                var newTeacherId = dto.TeacherId.Value;

                project.TeacherId = newTeacherId;

                var student = await _userRepository.GetStudentByUserIdAsync(project.Student.UserId);
                if (student != null)
                {
                    var oldTeacher = await _userRepository.GetTeacherByIdAsync(oldTeacherId);
                    var newTeacher = await _userRepository.GetTeacherByIdAsync(newTeacherId);

                    student.TeacherId = newTeacherId;
                    await _userRepository.UpdateUserAsync(student.User);

                    var studentFullName = $"{student.User.Surname} {student.User.Name} {student.User.Patronymic}".Trim();
                    var oldCuratorFullName = oldTeacher != null
                        ? $"{oldTeacher.User.Surname} {oldTeacher.User.Name} {oldTeacher.User.Patronymic}".Trim()
                        : "неизвестный преподаватель";
                    var newCuratorFullName = newTeacher != null
                        ? $"{newTeacher.User.Surname} {newTeacher.User.Name} {newTeacher.User.Patronymic}".Trim()
                        : "неизвестный преподаватель";

                    if (oldTeacher != null)
                    {
                        await _notificationService.SendStudentCuratorChangedForOldCuratorNotificationAsync(
                            oldTeacher.UserId, studentFullName);
                    }

                    if (newTeacher != null)
                    {
                        await _notificationService.SendStudentCuratorChangedForNewCuratorNotificationAsync(
                            newTeacher.UserId, studentFullName);
                    }

                    await _notificationService.SendStudentCuratorChangedForStudentNotificationAsync(
                        student.UserId, oldCuratorFullName, newCuratorFullName);
                }
            }

            if (dto.StatusId.HasValue)
                project.StatusId = dto.StatusId.Value;

            if (dto.TypeId.HasValue)
                project.TypeId = dto.TypeId.Value;

            if (dto.SubjectId.HasValue)
                project.SubjectId = dto.SubjectId.Value;

            if (dto.IsPublic.HasValue)
                project.IsPublic = dto.IsPublic.Value;

            if (dto.DateDeadline.HasValue)
            {
                project.DateDeadline = dto.DateDeadline.Value;
                project.Year = dto.DateDeadline.Value.Year;
            }

            await _projectRepository.UpdateProjectAsync(project);

            var updatedProject = await _projectRepository.GetProjectByIdAsync(projectId);
            return _mapper.Map<ProjectDto>(updatedProject);
        }
        public async Task<ProjectDto> UpdateStudentProjectAsync(int projectId, UpdateStudentProjectRequestDto dto, int studentUserId)
        {
            var project = await _projectRepository.GetProjectByIdAsync(projectId);

            if (project == null)
                throw new ProjectNotFoundException(projectId);

            var inactiveStatuses = new[] { 5, 6, 7 };
            if (inactiveStatuses.Contains(project.StatusId))
                throw new InvalidOperationException("Нельзя изменять проект, который уже защищён, отклонён или архивирован.");
            var student = await _userRepository.GetStudentByUserIdAsync(studentUserId);
            if (student == null || project.StudentId != student.Id)
                throw new ProjectAccessDeniedException();

            bool fileUploaded = false;
            bool documentationUploaded = false;
            bool visibilityChanged = false;
            bool? oldVisibility = null; 
            int oldStatusId = project.StatusId;


            if (dto.NewProjectFile != null)
            {
  
                if (!string.IsNullOrEmpty(project.FileProject))
                    _fileService.DeleteProjectFile(project.FileProject);

                project.FileProject = await _fileService.SaveProjectFileAsync(dto.NewProjectFile);
                fileUploaded = true;
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
                documentationUploaded = true;
            }
            else if (dto.RemoveDocumentationFile == true && !string.IsNullOrEmpty(project.FileDocumentation))
            {
                _fileService.DeleteDocumentationFile(project.FileDocumentation);
                project.FileDocumentation = null;
            }


            if (dto.IsPublic.HasValue)
            {
                oldVisibility = project.IsPublic;
                project.IsPublic = dto.IsPublic.Value;
                if (oldVisibility != dto.IsPublic.Value)
                    visibilityChanged = true;
                
            }

            if ((fileUploaded || documentationUploaded) && project.StatusId == 1)
                project.StatusId = 2;
            await _projectRepository.UpdateProjectAsync(project);

            if (fileUploaded && !documentationUploaded)
            {
                var studentUser = await _userRepository.GetByIdAsync(studentUserId);
                var studentFullName = $"{studentUser.Surname} {studentUser.Name} {studentUser.Patronymic}".Trim();
                var curatorUserId = project.Teacher?.User?.Id ?? 0;
                if (curatorUserId > 0)
                {
                    await _notificationService.SendProjectFileUpdatedNotificationAsync(
                        curatorUserId,
                        studentFullName,
                        project.Title
                    );
                }
            }
            else if (documentationUploaded && !fileUploaded)
            {
                var studentUser = await _userRepository.GetByIdAsync(studentUserId);
                var studentFullName = $"{studentUser.Surname} {studentUser.Name} {studentUser.Patronymic}".Trim();
                var curatorUserId = project.Teacher?.User?.Id ?? 0;
                if (curatorUserId > 0)
                {
                    await _notificationService.SendProjectDocumentationUpdatedNotificationAsync(
                        curatorUserId,
                        studentFullName,
                        project.Title
                    );
                }
            }
            else if(documentationUploaded && fileUploaded)
            {
                var studentUser = await _userRepository.GetByIdAsync(studentUserId);
                var studentFullName = $"{studentUser.Surname} {studentUser.Name} {studentUser.Patronymic}".Trim();
                var curatorUserId = project.Teacher?.User?.Id ?? 0;
                if (curatorUserId > 0)
                {
                    await _notificationService.SendProjectDocumentationAndFileUpdatedNotificationAsync(
                        curatorUserId,
                        studentFullName,
                        project.Title
                    );
                }
            }
            if (visibilityChanged)
            {
                var studentUser = await _userRepository.GetByIdAsync(studentUserId);
                var studentFullName = $"{studentUser.Surname} {studentUser.Name} {studentUser.Patronymic}".Trim();
                var curatorUserId = project.Teacher?.User?.Id ?? 0;
                var allUsers = await _userRepository.GetAllAsync();
                var adminUserIds = allUsers
                    .Where(u => u.IsAdmin)
                    .Select(u => u.Id)
                    .ToList();
                if (curatorUserId > 0)
                {
                    await _notificationService.SendProjectVisibilityChangedNotificationAsync(
                        curatorUserId,
                        studentFullName,
                        project.Title,
                        project.IsPublic,
                        adminUserIds
                    );
                }
            }
            var updatedProject = await _projectRepository.GetProjectByIdAsync(projectId);
            return _mapper.Map<ProjectDto>(updatedProject);
        }
        public async Task DeleteProjectAsync(int projectId)
        {
            var project = await _projectRepository.GetProjectByIdAsync(projectId);
            if (project == null)
                throw new ArgumentException("Проект не найден");
            var projectTitle = project.Title;
            var curatorUserId = project.Teacher?.User?.Id ?? 0;
            var studentUserId = project.Student?.User?.Id ?? 0;
            var studentFullName = $"{project.Student?.User?.Surname} {project.Student?.User?.Name} {project.Student?.User?.Patronymic}".Trim();


            if (!string.IsNullOrEmpty(project.FileProject))
                _fileService.DeleteProjectFile(project.FileProject);

     
            if (!string.IsNullOrEmpty(project.FileDocumentation))
                _fileService.DeleteDocumentationFile(project.FileDocumentation);


            if (project.Comments != null && project.Comments.Any())
                project.Comments.Clear(); 

    
            await _projectRepository.DeleteProjectAsync(project);
            if (curatorUserId > 0)
            {
                await _notificationService.SendDeleteProjectForCuratorNotificationAsync(
                    curatorUserId,
                    
                    studentFullName,
                    projectTitle
                );
            }
            if (studentUserId > 0)
            {
                await _notificationService.SendDeleteProjectForStudentNotificationAsync(
                    studentUserId,
                    projectTitle
                );
            }


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

            var inactiveStatuses = new[] { 5, 6, 7 };
            if (inactiveStatuses.Contains(project.StatusId))
                throw new InvalidOperationException("Нельзя комментировать проект, который уже защищён, отклонён или архивирован.");
            if (project.TeacherId != user.Teacher.Id)
                throw new AccessDeniedException("Вы можете комментировать только проекты своих студентов.");

            var comment = new Comment
            {
                Text = text,
                Date = DateTime.Now,
                UserId = user.Id,
                ProjectId = project.Id
            };

   
            project.Comments.Add(comment);


            await _projectRepository.UpdateProjectAsync(project);
            var student = await _userRepository.GetByIdAsync(project.Student.UserId);
            if (student != null)
            {
                var curatorFullName = $"{user.Surname} {user.Name} {user.Patronymic}".Trim();
                await _notificationService.SendCommentNotificationAsync(
                    student.Id,
                    curatorFullName,
                    project.Title,
                    text
                );
            }
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
            bool titleChanged = false;
            bool typeChanged = false;
            bool deadlineChanged = false;
            bool subjectChanged = false;
            string? oldTitle = null;
            string? newTitle = null;
            string? oldTypeName = null;
            string? newTypeName = null;
            string? oldSubjectName = null;
            string? newSubjectName = null;
            DateTime? oldDeadline = null;
            DateTime? newDeadline = null;
            if (!string.IsNullOrWhiteSpace(dto.Title))
            {
                oldTitle = project.Title;
                newTitle = dto.Title;

                if (oldTitle != newTitle)
                {
                    project.Title = newTitle;
                    titleChanged = true;
                }
            }
            if (dto.TypeId.HasValue)
            {
                var oldTypeId = project.TypeId;

                if (oldTypeId != dto.TypeId.Value)
                {
                    var oldType = await _directoryRepository.GetTypeByIdAsync(oldTypeId);
                    oldTypeName = oldType?.Name ?? "неизвестный тип";
                    project.TypeId = dto.TypeId.Value;
                    typeChanged = true;
                    var newType = await _directoryRepository.GetTypeByIdAsync(project.TypeId);
                    newTypeName = newType?.Name ?? "неизвестный тип";
                }
            }
            
            if (dto.SubjectId.HasValue)
            {
                var oldSubjectId = project.SubjectId;
                if (oldSubjectId != dto.SubjectId.Value)
                {
                    var oldSubject = await _directoryRepository.GetSubjectByIdAsync(oldSubjectId);
                    oldSubjectName = oldSubject?.Name ?? "неизвестный предмет";
                    project.SubjectId = dto.SubjectId.Value;
                    subjectChanged = true;
                    var newSubject = await _directoryRepository.GetSubjectByIdAsync(project.SubjectId);
                    newSubjectName = newSubject?.Name ?? "неизвестный предмет";
                }
            }
            if (dto.DateDeadline.HasValue)
            {
                oldDeadline = project.DateDeadline;
                newDeadline = dto.DateDeadline.Value;

                if (oldDeadline.Value.Date != newDeadline.Value.Date)
                {
                    project.DateDeadline = newDeadline.Value;
                    project.Year = newDeadline.Value.Year;
                    deadlineChanged = true;
                }
            }
            await _projectRepository.UpdateProjectAsync(project);
            var studentUserId = project.Student?.User?.Id ?? 0;
            var curatorFullName = $"{teacher.Surname} {teacher.Name} {teacher.Patronymic}".Trim();

            if (titleChanged)
            {
                
                if (studentUserId > 0)
                {
                    await _notificationService.SendProjectTitleChangedNotificationAsync(
                        studentUserId,
                        curatorFullName,
                        newTitle!,
                        oldTitle!
                    );
                }
            }
            if (typeChanged)
            {
                
                
                if (studentUserId > 0)
                {
                    await _notificationService.SendProjectTypeChangedNotificationAsync(
                        studentUserId,
                        curatorFullName,
                        newTypeName!,
                        oldTypeName!,
                        project.Title
                    );
                }
            }
            if (subjectChanged)
            {
                

                if (studentUserId > 0)
                {
                    await _notificationService.SendProjectSubjectChangedNotificationAsync(
                        studentUserId,
                        curatorFullName,
                        newSubjectName!,
                        oldSubjectName!,
                        project.Title
                    );
                }
            }
            if (deadlineChanged && studentUserId > 0 && newDeadline.HasValue)
            {
                await _notificationService.SendProjectDeadlineChangedNotificationAsync(
                    studentUserId,
                    curatorFullName,
                    project.Title,
                    newDeadline.Value
                );
            }
            var updatedProject = await _projectRepository.GetProjectByIdAsync(projectId);
            return _mapper.Map<ProjectDto>(updatedProject);
        }



    }
}
