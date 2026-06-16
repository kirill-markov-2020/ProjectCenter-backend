using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCenter.Application.DTOs.Project;
using ProjectCenter.Application.DTOs;
using ProjectCenter.Application.Interfaces;
using ProjectCenter.Core.Enums;
using System.Security.Claims;

namespace ProjectCenter.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }


        [HttpGet]
        public async Task<IActionResult> GetProjects(
            [FromQuery] string? searchText,
            [FromQuery] int? year,
            [FromQuery] int? groupId,
            [FromQuery] ProjectSortBy? sortBy)
        {
            if (!HttpContext.Items.ContainsKey("UserId"))
                return Unauthorized();

            int userId = (int)HttpContext.Items["UserId"];

            var projects = await _projectService.GetProjectsForUserAsync(
                userId, searchText, year, groupId, sortBy);

            return Ok(projects);
        }

        [HttpGet("groups/by-year")]
        public async Task<IActionResult> GetGroupsByYear([FromQuery] int year)
        {
            var groups = await _projectService.GetAvailableGroupsForYearAsync(year);
            return Ok(groups);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            return Ok(project);
        }
        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequestDto dto)
        {
            if (!HttpContext.Items.ContainsKey("UserId"))
                return Unauthorized();

            int userId = (int)HttpContext.Items["UserId"];

            var project = await _projectService.CreateProjectAsync(dto, userId);
            return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, project);
        }
        [HttpPost("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProjectByAdmin([FromBody] AdminCreateProjectRequestDto dto)
        {
            if (dto.StudentUserId <= 0)
                return BadRequest(new { error = "Необходимо указать StudentUserId" });

            if (dto.CreatedDate == default)
                return BadRequest(new { error = "Необходимо указать дату создания проекта" });

            if (dto.DateDeadline == default)
                return BadRequest(new { error = "Необходимо указать дату дедлайна" });

            if (dto.DateDeadline <= dto.CreatedDate)
                return BadRequest(new { error = "Дата дедлайна должна быть позже даты создания" });

            var project = await _projectService.CreateProjectByAdminAsync(dto);
            return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, project);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] UpdateProjectRequestDto dto)
        {
            var updatedProject = await _projectService.UpdateProjectAsync(id, dto);
            return Ok(updatedProject);
        }
        [HttpPut("my/{id}")]
        [Authorize(Roles = "Student")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateMyProject(int id, [FromForm] UpdateStudentProjectRequestDto dto)
        {
            if (!HttpContext.Items.ContainsKey("UserId"))
                return Unauthorized();

            int userId = (int)HttpContext.Items["UserId"];

            var updatedProject = await _projectService.UpdateStudentProjectAsync(id, dto, userId);
            return Ok(updatedProject);
        }
        [HttpDelete("{id}")]
        
        public async Task<IActionResult> DeleteProject(int id)
        {
            await _projectService.DeleteProjectAsync(id);
            return NoContent();
        }
        [HttpGet("my")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMyProject()
        {
            if (!HttpContext.Items.ContainsKey("UserId"))
                return Unauthorized();

            int userId = (int)HttpContext.Items["UserId"];

            var project = await _projectService.GetMyProjectAsync(userId);

            if (project == null)
            {
                return NotFound(new { message = "У вас нет активного проекта" });
            }

            return Ok(project);
        }
        [HttpPost("{id}/comments")]
        [Authorize]
        public async Task<IActionResult> AddComment(int id, [FromBody] CreateCommentRequestDto dto)
        {
            if (!HttpContext.Items.ContainsKey("UserId"))
                return Unauthorized();

            int userId = (int)HttpContext.Items["UserId"];

            await _projectService.AddCommentAsync(id, userId, dto.Text);

            return Ok(new { message = "Комментарий добавлен" });
        }



    }
}
