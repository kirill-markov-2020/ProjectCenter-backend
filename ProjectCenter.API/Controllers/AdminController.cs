using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCenter.Application.Interfaces;

namespace ProjectCenter.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var dashboard = await _adminService.GetDashboardAsync();
            return Ok(dashboard);
        }

        [HttpGet("last-projects")]
        public async Task<IActionResult> GetLastProjects([FromQuery] int count = 5)
        {
            var projects = await _adminService.GetLastProjectsAsync(count);
            return Ok(projects);
        }

        [HttpGet("active-teachers")]
        public async Task<IActionResult> GetActiveTeachers()
        {
            var teachers = await _adminService.GetActiveTeachersAsync();
            return Ok(teachers);
        }

        [HttpGet("recent-activity")]
        public async Task<IActionResult> GetRecentActivity([FromQuery] int count = 5)
        {
            var activity = await _adminService.GetRecentActivityAsync(count);
            return Ok(activity);
        }
    }
}
