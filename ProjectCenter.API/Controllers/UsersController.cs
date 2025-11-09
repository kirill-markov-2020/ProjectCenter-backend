using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectCenter.Application.DTOs;
using ProjectCenter.Application.DTOs.CreateUser;
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

        // POST api/users
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto dto)
        {
            var result = await _userService.CreateUserAsync(dto);
            return CreatedAtAction(null, new { id = result.UserId }, result);
        }
    }
}
