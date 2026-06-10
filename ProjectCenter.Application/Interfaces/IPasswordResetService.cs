using ProjectCenter.Application.DTOs.Auth;

namespace ProjectCenter.Application.Interfaces
{
    public interface IPasswordResetService
    {
        Task<ResetPasswordResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto dto);
        Task<ResetPasswordResponseDto> VerifyCodeAsync(VerifyResetCodeRequestDto dto);
        Task<ResetPasswordResponseDto> ResetPasswordAsync(ResetPasswordRequestDto dto);
    }
}