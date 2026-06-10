using Microsoft.AspNetCore.Mvc;
using ProjectCenter.Application.DTOs.Auth;
using ProjectCenter.Application.Interfaces;

namespace ProjectCenter.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PasswordResetController : ControllerBase
    {
        private readonly IPasswordResetService _passwordResetService;

        public PasswordResetController(IPasswordResetService passwordResetService)
        {
            _passwordResetService = passwordResetService;
        }

        [HttpPost("forgot")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto dto)
        {
            var result = await _passwordResetService.ForgotPasswordAsync(dto);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyResetCodeRequestDto dto)
        {
            var result = await _passwordResetService.VerifyCodeAsync(dto);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto dto)
        {
            var result = await _passwordResetService.ResetPasswordAsync(dto);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);
        }
    }
}