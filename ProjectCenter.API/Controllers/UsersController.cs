using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCenter.Application.DTOs.User;
using ProjectCenter.Application.Interfaces;

namespace ProjectCenter.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto dto)
        {
            var result = await _userService.CreateUserAsync(dto);
            return CreatedAtAction(null, new { id = result.UserId }, result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id, [FromBody] DeleteUserRequestDto? dto = null)
        {
            await _userService.DeleteUserAsync(id, dto);
            return NoContent();
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveUsers()
        {
            var users = await _userService.GetActiveUsersAsync();
            return Ok(users);
        }

        [HttpGet("graduated")]
        public async Task<IActionResult> GetGraduatedUsers()
        {
            var users = await _userService.GetGraduatedUsersAsync();
            return Ok(users);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequestDto dto)
        {
            if (dto == null)
                return BadRequest(new { error = "Данные для обновления не переданы." });

            
            var role = HttpContext.Items["UserRole"]?.ToString();
            if (role != "Admin")
                return Forbid();

            await _userService.UpdateUserByAdminAsync(id, dto);

            return NoContent();
        }
        [HttpGet("students")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _userService.GetAllStudentsAsync();
            return Ok(students);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }



    }
}
