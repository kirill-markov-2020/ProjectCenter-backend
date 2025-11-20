using Microsoft.AspNetCore.Http;
using ProjectCenter.Core.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ProjectCenter.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            switch (exception)
            {
                case InvalidRoleException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsJsonAsync(new { error = ex.Message, statusCode = 400 });
                    break;

                case InvalidPhoneNumberException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsJsonAsync(new { error = ex.Message, statusCode = 400 });
                    break;

                case InvalidEmailException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsJsonAsync(new { error = ex.Message, statusCode = 400 });
                    break;

                case InvalidPasswordException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsJsonAsync(new { error = ex.Message, statusCode = 400 });
                    break;

                case InvalidStudentDataException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsJsonAsync(new { error = ex.Message, statusCode = 400 });
                    break;
                case InvalidOperationException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsJsonAsync(new { error = ex.Message, statusCode = 400 });
                    break;
                case UserNotFoundException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    await context.Response.WriteAsJsonAsync(new { error = ex.Message, statusCode = 404 });
                    break;

                case ArgumentException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsJsonAsync(new { error = ex.Message, statusCode = 400 });
                    break;
                case TeacherHasStudentsException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsJsonAsync(new { error = ex.Message, statusCode = 400 });
                    break;

                case TokenExpiredException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsJsonAsync(new { error = ex.Message, statusCode = 401 });
                    break;
                case ProjectNotFoundException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    await context.Response.WriteAsJsonAsync(new { error = ex.Message, statusCode = 404 });
                    break;

                case StudentNotFoundException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    await context.Response.WriteAsJsonAsync(new { error = ex.Message, statusCode = 404 });
                    break;
                case ActiveProjectExistsException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict; 
                    await context.Response.WriteAsJsonAsync(new { error = ex.Message, statusCode = 409 });
                    break;
                case FileValidationException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsJsonAsync(new { error = ex.Message, statusCode = 400 });
                    break;

                case ProjectAccessDeniedException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    await context.Response.WriteAsJsonAsync(new { error = ex.Message, statusCode = 403 });
                    break;


                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    await context.Response.WriteAsJsonAsync(new { error = "Произошла внутренняя ошибка сервера.", statusCode = 500 });
                    break;
            }
        }
    }
}
