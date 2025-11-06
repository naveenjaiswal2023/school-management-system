using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Domain.Exceptions;

namespace SchoolManagement.API.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = new ProblemDetails();

            switch (exception)
            {
                case BiometricVerificationException:
                    response.Status = StatusCodes.Status400BadRequest;
                    response.Title = "Biometric Verification Failed";
                    break;
                case AttendanceException:
                    response.Status = StatusCodes.Status400BadRequest;
                    response.Title = "Attendance Processing Error";
                    break;
                case NotificationException:
                    response.Status = StatusCodes.Status500InternalServerError;
                    response.Title = "Notification Service Error";
                    break;
                case ArgumentNullException:
                    response.Status = StatusCodes.Status400BadRequest;
                    response.Title = "Invalid Input";
                    break;
                case UnauthorizedAccessException:
                    response.Status = StatusCodes.Status403Forbidden;
                    response.Title = "Access Denied";
                    break;
                default:
                    response.Status = StatusCodes.Status500InternalServerError;
                    response.Title = "Internal Server Error";
                    break;
            }

            response.Detail = exception.Message;
            context.Response.StatusCode = response.Status.Value;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
        }
    }
}
