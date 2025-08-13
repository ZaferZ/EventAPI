using EventAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace EventAPI.Middleware
{
    public class GlobalExceptionMiddleware
    {
        /// <summary>
        /// Middleware that handles all unhandled exceptions globally.
        /// Converts exceptions into standardized <see cref="ProblemDetails"/> responses.
        /// </summary>

        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env; // This is used to determine if the environment is development or production

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the ASP.NET Core pipeline.</param>
        /// <param name="logger">Logger to record exceptions.</param>
        /// <param name="env">Hosting environment used to control detailed error messages.</param>
        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        /// <summary>
        /// Invokes the middleware for the current HTTP context.
        /// </summary>
        /// <param name="context">The HTTP context for the request.</param>
        /// <returns>A <see cref="Task"/> representing the completion of request handling.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = EnsureCorrelationId(context);

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var (status, title, detail, errors, logLevel) = MapException(ex);

                LogWithLevel(_logger, ex, logLevel, correlationId);

                var problem = new ProblemDetails
                {
                    Status = (int)status,
                    Title = title,
                    Detail = _env.IsDevelopment() ? detail : null,
                    Instance = context.Request.Path
                };

                problem.Extensions["correlationId"] = correlationId;

                if (errors is not null)
                    problem.Extensions["errors"] = errors;

                context.Response.StatusCode = problem.Status ?? (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/problem+json";

                var json = JsonSerializer.Serialize(problem, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                await context.Response.WriteAsync(json);
            }
        }


        /// <summary>
        /// Ensures that the HTTP request has a correlation ID.
        /// If none exists, generates a new GUID and adds it to the response headers.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>The correlation ID.</returns>
        private static string EnsureCorrelationId(HttpContext context)
        {
            const string header = "X-Correlation-ID";
            if (!context.Request.Headers.TryGetValue(header, out var value) || string.IsNullOrWhiteSpace(value))
            {
                value = Guid.NewGuid().ToString("n");
            }
            context.Response.Headers[header] = value!;
            return value!;
        }

        /// <summary>
        /// Maps a given exception to HTTP status code, title, detail, errors, and log level.
        /// </summary>
        /// <param name="ex">The exception to map.</param>
        /// <returns>A tuple containing status code, title, detail message, optional validation errors, and log level.</returns>
        private static (HttpStatusCode status, string title, string detail,
                IDictionary<string, string[]>? errors, LogLevel logLevel)
        MapException(Exception ex)
        {
            return ex switch
            {
                NotFoundException nf => (HttpStatusCode.NotFound, "Resource not found", nf.Message, null, LogLevel.Information),
                ForbidenException fb => (HttpStatusCode.Forbidden, "Forbidden", fb.Message, null, LogLevel.Warning),
                ConflictException cf => (HttpStatusCode.Conflict, "Conflict", cf.Message, null, LogLevel.Warning),
                DomainValidationException dv =>
                    (HttpStatusCode.BadRequest, "Validation failed",
                     "One or more validation errors occurred.", dv.Errors, LogLevel.Warning),

                UnauthorizedAccessException ua =>
                    (HttpStatusCode.Unauthorized, "Unauthorized", ua.Message, null, LogLevel.Warning),

                _ => (HttpStatusCode.InternalServerError, "Server error", ex.Message, null, LogLevel.Error)
            };
        }

        /// <summary>
        /// Logs the exception with a specified <see cref="LogLevel"/> and correlation ID.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="ex">The exception to log.</param>
        /// <param name="level">The log severity level.</param>
        /// <param name="correlationId">The correlation ID for tracing.</param>
        private static void LogWithLevel(ILogger logger, Exception ex, LogLevel level, string correlationId)
        {
            logger.Log(level, ex,
                "Exception handled by GlobalExceptionMiddleware. CorrelationId: {CorrelationId}",
                correlationId);
        }

      
    }
    public static class GlobalExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
