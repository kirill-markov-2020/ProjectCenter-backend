using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCenter.Application.DTOs.UpdateProject;
using ProjectCenter.Application.Interfaces;
using ProjectCenter.Application.Services;

namespace ProjectCenter.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;
        private readonly IProjectService _projectService;

        public TeacherController(ITeacherService teacherService, IProjectService projectService)
        {
            _teacherService = teacherService;
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTeachers()
        {
            var teachers = await _teacherService.GetAllTeachersAsync();
            return Ok(teachers);
        }
        [HttpGet("students")]
        
        public async Task<IActionResult> GetMyStudents()
        {
            if (!HttpContext.Items.ContainsKey("UserId"))
                return Unauthorized();

            int userId = (int)HttpContext.Items["UserId"];

            var data = await _teacherService.GetMyStudentsAsync(userId);

            return Ok(data);
        }
        [HttpGet("students/projects/{projectId}")]
        public async Task<IActionResult> GetStudentProjectById(int projectId)
        {
            if (!HttpContext.Items.ContainsKey("UserId"))
                return Unauthorized();

            int userId = (int)HttpContext.Items["UserId"];

            var project = await _projectService.GetTeacherStudentProjectAsync(projectId, userId);
            return Ok(project);
        }
        [HttpPut("students/projects/{projectId}")]
        public async Task<IActionResult> UpdateStudentProject(int projectId, [FromBody] UpdateTeacherProjectRequestDto dto)
        {
            if (!HttpContext.Items.ContainsKey("UserId"))
                return Unauthorized();

            int userId = (int)HttpContext.Items["UserId"];

            var updatedProject = await _projectService.UpdateProjectByTeacherAsync(projectId, dto, userId);
            return Ok(updatedProject);
        }
    }
    }

