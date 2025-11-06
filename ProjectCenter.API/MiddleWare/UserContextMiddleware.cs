using System.Security.Claims;

namespace ProjectCenter.API.Middleware
{
    public class UserContextMiddleware
    {
        private readonly RequestDelegate _next;

        public UserContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User?.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var role = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (int.TryParse(userId, out var parsedUserId))
                {
                    context.Items["UserId"] = parsedUserId;
                    context.Items["UserRole"] = role;
                }
            }

            await _next(context);
        }
    }
}
