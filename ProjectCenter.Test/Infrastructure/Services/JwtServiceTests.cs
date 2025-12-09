using Moq;
using Xunit;
using Microsoft.Extensions.Configuration;
using ProjectCenter.Infrastructure.Services;
using ProjectCenter.Core.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace ProjectCenter.Test.Infrastructure.Services
{
    public class JwtServiceTests
    {
        [Fact]
        public void GenerateToken_ShouldIncludeStudentRole_ForStudentUser()
        {
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(config => config["Jwt:Key"])
                .Returns("test_secret_key_32_characters_long!");
            mockConfiguration.Setup(config => config["Jwt:Issuer"])
                .Returns("ProjectCenterTest");
            mockConfiguration.Setup(config => config["Jwt:Audience"])
                .Returns("TestClient");

            var jwtService = new JwtService(mockConfiguration.Object);

            var studentUser = new User
            {
                Id = 123,
                Login = "student_login",
                IsAdmin = false,
                Student = new Student { Id = 1, UserId = 123 }
            };

            string token = jwtService.GenerateToken(studentUser);
            var tokenHandler = new JwtSecurityTokenHandler();
            var decodedToken = tokenHandler.ReadJwtToken(token);
            var roleClaim = decodedToken.Claims
                .FirstOrDefault(c => c.Type == "role")?.Value;
            Assert.Equal("Student", roleClaim);

            var userIdClaim = decodedToken.Claims
                .FirstOrDefault(c => c.Type == "nameid")?.Value;
            Assert.Equal("123", userIdClaim);
        }
    }
}