using AutoMapper;
using ProjectCenter.Application.DTOs.Auth;
using ProjectCenter.Application.Interfaces;
using ProjectCenter.Core.Entities;
using ProjectCenter.Core.ValueObjects;

namespace ProjectCenter.Application.Services
{
    public class PasswordResetService : IPasswordResetService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IPasswordResetRepository _passwordResetRepository;

        public PasswordResetService(IUserRepository userRepository, IEmailService emailService, IPasswordResetRepository passwordResetRepository)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _passwordResetRepository = passwordResetRepository;
        }
        public async Task<ResetPasswordResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null)
            {
                return new ResetPasswordResponseDto
                {
                    Success = false,
                    Message = "Пользователь с таким email не найден"
                };
            }

            var code = new Random().Next(100000, 999999).ToString();

            var resetCode = new PasswordResetCode
            {
                Email = dto.Email,
                Code = code,
                CreatedAt = DateTime.Now,
                ExpiresAt = DateTime.Now.AddMinutes(15),
                IsUsed = false
            };

            await _passwordResetRepository.AddAsync(resetCode);

            var body = $"Ваш код для восстановления пароля: {code}\n\nКод действителен 15 минут.";
            await _emailService.SendEmailAsync(dto.Email, "Восстановление пароля", body);

            return new ResetPasswordResponseDto
            {
                Success = true,
                Message = "Код восстановления отправлен на ваш email"
            };
        }

        public async Task<ResetPasswordResponseDto> VerifyCodeAsync(VerifyResetCodeRequestDto dto)
        {
            var resetCode = await _passwordResetRepository.GetByEmailAndCodeAsync(dto.Email, dto.Code);

            if (resetCode == null)
            {
                return new ResetPasswordResponseDto
                {
                    Success = false,
                    Message = "Неверный код или код уже использован"
                };
            }

            if (resetCode.ExpiresAt < DateTime.Now)
            {
                return new ResetPasswordResponseDto
                {
                    Success = false,
                    Message = "Срок действия кода истёк. Запросите новый код"
                };
            }

            var resetToken = Guid.NewGuid().ToString();

            return new ResetPasswordResponseDto
            {
                Success = true,
                Message = "Код подтверждён",
                ResetToken = resetToken
            };
        }

        public async Task<ResetPasswordResponseDto> ResetPasswordAsync(ResetPasswordRequestDto dto)
        {
            if (dto.NewPassword != dto.ConfirmPassword)
            {
                return new ResetPasswordResponseDto
                {
                    Success = false,
                    Message = "Пароли не совпадают"
                };
            }

            var passwordErrors = PasswordValidator.Validate(dto.NewPassword);
            if (passwordErrors.Any())
            {
                return new ResetPasswordResponseDto
                {
                    Success = false,
                    Message = string.Join(" ", passwordErrors)
                };
            }

            var resetCode = await _passwordResetRepository.GetByEmailAndCodeAsync(dto.Email, dto.Code);

            if (resetCode == null)
            {
                return new ResetPasswordResponseDto
                {
                    Success = false,
                    Message = "Неверный код или код уже использован"
                };
            }

            if (resetCode.ExpiresAt < DateTime.Now)
            {
                return new ResetPasswordResponseDto
                {
                    Success = false,
                    Message = "Срок действия кода истёк. Запросите новый код"
                };
            }

            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null)
            {
                return new ResetPasswordResponseDto
                {
                    Success = false,
                    Message = "Пользователь не найден"
                };
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _userRepository.UpdateUserAsync(user);

            await _passwordResetRepository.MarkAsUsedAsync(resetCode.Id);

            return new ResetPasswordResponseDto
            {
                Success = true,
                Message = "Пароль успешно изменён"
            };
        }
    }
}