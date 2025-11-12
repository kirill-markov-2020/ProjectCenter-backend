using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCenter.Application.DTOs;
using ProjectCenter.Application.DTOs.CreateUser;
using ProjectCenter.Application.DTOs.UpdateUser;
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
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequestDto dto)
        {
            var currentRole = HttpContext.Items["UserRole"]?.ToString();
            if (!int.TryParse(HttpContext.Items["UserId"]?.ToString(), out var currentUserId))
            {
                return Unauthorized();
            }

            var updated = await _userService.UpdateUserAsync(id, dto, currentRole ?? "User", currentUserId);
            return Ok(updated);
        }


    }
}
