using ProductManagment.Domain.Constants;
using System.Net;

namespace ProductManagement.API.Middleware
{
    /// <summary>
    /// Global error handling middleware for catching and formatting exceptions
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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
                _logger.LogError(ex, "An unhandled exception has occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new { message = "", statusCode = 0 };

            switch (exception)
            {
                case KeyNotFoundException:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    response = new { message = exception.Message, statusCode = StatusCodes.Status404NotFound };
                    break;

                case ArgumentException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response = new { message = exception.Message, statusCode = StatusCodes.Status400BadRequest };
                    break;

                case InvalidOperationException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response = new { message = exception.Message, statusCode = StatusCodes.Status400BadRequest };
                    break;

                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    response = new { message = ValidationMessages.InternalServerError, statusCode = StatusCodes.Status500InternalServerError };
                    break;
            }

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
