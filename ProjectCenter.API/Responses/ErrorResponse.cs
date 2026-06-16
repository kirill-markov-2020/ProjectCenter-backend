using Microsoft.AspNetCore.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectCenter.API.Responses
{
    public record ErrorResponse
    {
        public string Error { get; init; }
        public int StatusCode { get; init; }
        public string? Detail { get; init; }  
        public string? Type { get; init; }    

        public ErrorResponse(string error, int statusCode, string? detail = null, string? type = null)
        {
            Error = error;
            StatusCode = statusCode;
            Detail = detail;
            Type = type ?? GetErrorType(statusCode);
        }

        private static string GetErrorType(int statusCode) => statusCode switch
        {
            400 => "BadRequest",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "NotFound",
            409 => "Conflict",
            500 => "InternalServerError",
            _ => "UnknownError"
        };
    }
}
