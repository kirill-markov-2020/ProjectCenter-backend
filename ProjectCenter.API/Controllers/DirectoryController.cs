using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCenter.Application.Interfaces;

namespace ProjectCenter.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DirectoryController : ControllerBase
    {
        private readonly IDirectoryService _directoryService;

        public DirectoryController(IDirectoryService directoryService)
        {
            _directoryService = directoryService;
        }

        [HttpGet("statuses")]
        public async Task<IActionResult> GetStatuses()
        {
            var data = await _directoryService.GetStatusesAsync();
            return Ok(data);
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetTypes()
        {
            var data = await _directoryService.GetTypesAsync();
            return Ok(data);
        }

        [HttpGet("subjects")]
        public async Task<IActionResult> GetSubjects()
        {
            var data = await _directoryService.GetSubjectsAsync();
            return Ok(data);
        }

        [HttpGet("groups")]
        public async Task<IActionResult> GetGroups()
        {
            var data = await _directoryService.GetGroupsAsync();
            return Ok(data);
        }
    }
}
