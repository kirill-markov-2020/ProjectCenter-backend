using Microsoft.AspNetCore.Http;
using ProjectCenter.API.Middleware;
using System.Security.Claims;
using Xunit;

namespace ProjectCenter.Tests.Unit.Api.Middleware
{
    public class UserContextMiddlewareTests
    {
        [Fact]
        public async Task InvokeAsync_ShouldExtractUserData_FromAuthenticatedUser()
        {
            var middleware = new UserContextMiddleware(
                innerHttpContext => Task.CompletedTask);

            var httpContext = new DefaultHttpContext();
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "456"),
                new Claim(ClaimTypes.Role, "Teacher")
            };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            httpContext.User = new ClaimsPrincipal(identity);
            await middleware.InvokeAsync(httpContext);
            Assert.Equal(456, httpContext.Items["UserId"]);
            Assert.Equal("Teacher", httpContext.Items["UserRole"]);
        }
    }
}