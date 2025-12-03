using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCenter.Application.DTOs.UpdateProject;
using ProjectCenter.Application.Interfaces;
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
        public async Task<IActionResult> GetProjects()
        {
            if (!HttpContext.Items.ContainsKey("UserId"))
                return Unauthorized();

            int userId = (int)HttpContext.Items["UserId"];
            string role = HttpContext.Items["UserRole"]?.ToString() ?? "User";

            bool isAdmin = role == "Admin";

            var projects = await _projectService.GetProjectsForUserAsync(userId, isAdmin);
            return Ok(projects);
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
        [Authorize(Roles = "Admin")]
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


    }
}
