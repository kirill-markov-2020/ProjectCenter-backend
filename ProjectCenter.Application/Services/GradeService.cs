using ProjectCenter.Application.DTOs.Grade;
using ProjectCenter.Application.Interfaces;
using ProjectCenter.Core.Entities;
using ProjectCenter.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCenter.Application.Services
{
    public class GradeService : IGradeService
    {
        private readonly IGradeRepository _gradeRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;

        public GradeService(
            IGradeRepository gradeRepository,
            IProjectRepository projectRepository,
            IUserRepository userRepository,
            INotificationService notificationService)
        {
            _gradeRepository = gradeRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
        }

   
        public async Task<GradeDto> SetGradeAsync(int teacherUserId, GradeRequestDto dto)
        {
        
            var teacher = await _userRepository.GetByIdAsync(teacherUserId);
            if (teacher == null || teacher.Teacher == null)
                throw new Exception("Вы не преподаватель");

      
            var project = await _projectRepository.GetProjectByIdAsync(dto.ProjectId);
            if (project == null)
                throw new ProjectNotFoundException(dto.ProjectId);

         
            if (project.TeacherId != teacher.Teacher.Id)
                throw new ProjectAccessDeniedException();

      
            if (dto.Value < 1 || dto.Value > 5)
                throw new ArgumentException("Оценка должна быть от 1 до 5");

          
            var existingGrade = await _gradeRepository.GetByProjectIdAsync(dto.ProjectId);

            if (existingGrade != null)
            {
         
                existingGrade.Value = dto.Value;
                existingGrade.Comment = dto.Comment;
                existingGrade.CreatedAt = DateTime.Now; 
                await _gradeRepository.UpdateAsync(existingGrade);

                return new GradeDto
                {
                    Value = existingGrade.Value,
                    Comment = existingGrade.Comment,
                    CreatedAt = existingGrade.CreatedAt
                };
            }

        
            var grade = new Grade
            {
                ProjectId = dto.ProjectId,
                TeacherId = teacher.Teacher.Id,
                Value = dto.Value,
                Comment = dto.Comment,
                CreatedAt = DateTime.Now
            };

            await _gradeRepository.AddAsync(grade);
            var teacherFullName = $"{teacher.Surname} {teacher.Name} {teacher.Patronymic}".Trim();
            var student = await _userRepository.GetByIdAsync(project.Student.UserId);
            if (student != null)
            {
                await _notificationService.SendAddGradeNotificationAsyns
                    (
                        student.Id,
                        teacherFullName,
                        project.Title,
                        dto.Value,
                        dto.Comment
                    );
            }
            return new GradeDto
            {
                Value = grade.Value,
                Comment = grade.Comment,
                CreatedAt = grade.CreatedAt
            };
            
        }
        public async Task<GradeDto> UpdateGradeAsync(int teacherUserId, int projectId, GradeRequestDto dto)
        {
      
            var teacher = await _userRepository.GetByIdAsync(teacherUserId);
            if (teacher == null || teacher.Teacher == null)
                throw new Exception("Вы не преподаватель");


            var project = await _projectRepository.GetProjectByIdAsync(projectId);
            if (project == null)
                throw new ProjectNotFoundException(projectId);

            if (project.TeacherId != teacher.Teacher.Id)
                throw new ProjectAccessDeniedException();

       
            if (dto.Value < 1 || dto.Value > 5)
                throw new ArgumentException("Оценка должна быть от 1 до 5");

            var existingGrade = await _gradeRepository.GetByProjectIdAsync(projectId);
            if (existingGrade == null)
                throw new Exception("Оценка ещё не выставлена. Сначала создайте оценку.");

            int oldValue = existingGrade.Value;
            existingGrade.Value = dto.Value;
            existingGrade.Comment = dto.Comment;
            existingGrade.CreatedAt = DateTime.Now;
            await _gradeRepository.UpdateAsync(existingGrade);
            var teacherFullName = $"{teacher.Surname} {teacher.Name} {teacher.Patronymic}".Trim();
            var student = await _userRepository.GetByIdAsync(project.Student.UserId);
            if (student != null)
            {
                await _notificationService.SendUpdateGradeNotificationAsyns
                    (
                        student.Id,
                        teacherFullName,
                        project.Title,
                        oldValue,
                        dto.Value,
                        dto.Comment
                    );
            }
            return new GradeDto
            {
                Value = existingGrade.Value,
                Comment = existingGrade.Comment,
                CreatedAt = existingGrade.CreatedAt
            };
        }

        public async Task<GradeDto> GetGradeByProjectIdAsync(int projectId)
        {
            var grade = await _gradeRepository.GetByProjectIdAsync(projectId);
            if (grade == null)
                return null;

            return new GradeDto
            {
                Value = grade.Value,
                Comment = grade.Comment,
                CreatedAt = grade.CreatedAt
            };
        }

        public async Task<bool> HasGradeAsync(int projectId)
        {
            var grade = await _gradeRepository.GetByProjectIdAsync(projectId);
            return grade != null;
        }

    }
    }

