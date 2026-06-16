using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCenter.Application.DTOs.Grade;
using ProjectCenter.Application.Interfaces;

namespace ProjectCenter.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GradesController : ControllerBase
    {
        private readonly IGradeService _gradeService;

        public GradesController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        [HttpPost]
        public async Task<IActionResult> SetGrade([FromBody] GradeRequestDto dto)
        {
            if (!HttpContext.Items.ContainsKey("UserId"))
                return Unauthorized();

            int userId = (int)HttpContext.Items["UserId"];

          
            if (await _gradeService.HasGradeAsync(dto.ProjectId))
                return BadRequest(new { error = "Оценка для этого проекта уже выставлена." });

            var result = await _gradeService.SetGradeAsync(userId, dto);
            return CreatedAtAction(nameof(GetGrade), new { projectId = dto.ProjectId }, result);
        }

      
        [HttpPut("{projectId}")]
        public async Task<IActionResult> UpdateGrade(int projectId, [FromBody] GradeRequestDto dto)
        {
            if (!HttpContext.Items.ContainsKey("UserId"))
                return Unauthorized();

            int userId = (int)HttpContext.Items["UserId"];

            dto.ProjectId = projectId;
            var result = await _gradeService.UpdateGradeAsync(userId, projectId, dto);
            return Ok(result);
        }

        
        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetGrade(int projectId)
        {
            var result = await _gradeService.GetGradeByProjectIdAsync(projectId);
            if (result == null)
                return NotFound(new { error = "Оценка для этого проекта ещё не выставлена." });

            return Ok(result);
        }
    }
}
