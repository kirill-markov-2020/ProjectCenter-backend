using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCenter.Application.DTOs;
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

    }
}
