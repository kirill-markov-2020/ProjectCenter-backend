using ProjectCenter.API.Responses;
using ProjectCenter.Core.Exceptions;

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

            var response = exception switch
            {
                InvalidRoleException ex => new ErrorResponse(ex.Message, 400, type: "InvalidRole"),
                InvalidPhoneNumberException ex => new ErrorResponse(ex.Message, 400, type: "InvalidPhoneNumber"),
                InvalidEmailException ex => new ErrorResponse(ex.Message, 400, type: "InvalidEmail"),
                InvalidPasswordException ex => new ErrorResponse(ex.Message, 400, type: "InvalidPassword"),
                InvalidStudentDataException ex => new ErrorResponse(ex.Message, 400, type: "InvalidStudentData"),
                InvalidOperationException ex => new ErrorResponse(ex.Message, 400, type: "InvalidOperation"),
                ArgumentException ex => new ErrorResponse(ex.Message, 400, type: "Argument"),
                FileValidationException ex => new ErrorResponse(ex.Message, 400, type: "FileValidation"),

                TokenExpiredException ex => new ErrorResponse(ex.Message, 401, type: "TokenExpired"),

                AccessDeniedException ex => new ErrorResponse(ex.Message, 403, type: "AccessDenied"),
                ProjectAccessDeniedException ex => new ErrorResponse(ex.Message, 403, type: "ProjectAccessDenied"),
                NoCuratorProjectException ex => new ErrorResponse(ex.Message, 403, type: "NoCuratorProject"),

                UserNotFoundException ex => new ErrorResponse(ex.Message, 404, type: "UserNotFound"),
                ProjectNotFoundException ex => new ErrorResponse(ex.Message, 404, type: "ProjectNotFound"),
                StudentNotFoundException ex => new ErrorResponse(ex.Message, 404, type: "StudentNotFound"),
                NoProjectsForTeacherException ex => new ErrorResponse(ex.Message, 404, type: "NoProjectsForTeacher"),

                ActiveProjectExistsException ex => new ErrorResponse(ex.Message, 409, type: "ActiveProjectExists"),

                TeacherHasStudentsException ex => new ErrorResponse(ex.Message, 400, type: "TeacherHasStudents"),

                _ => new ErrorResponse("Произошла внутренняя ошибка сервера.", 500, detail: exception.Message, type: "InternalServerError")
            };

            context.Response.StatusCode = response.StatusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}