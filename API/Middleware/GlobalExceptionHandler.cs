using Domain.Exceptions;
using Domain.Response;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API.Middleware
{
    /// <summary>
    /// Global exception handler that catches and processes all unhandled exceptions,
    /// including database errors.
    /// </summary>
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IWebHostEnvironment _environment;

        public GlobalExceptionHandler(
            ILogger<GlobalExceptionHandler> logger,
            IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            LogException(exception);

            var errorResponse = CreateErrorResponse(exception, httpContext);

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = errorResponse.StatusCode;

            await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

            return true;
        }

        private void LogException(Exception exception)
        {
            if (exception is NotFoundException or ValidationException or BadRequestException)
            {
                _logger.LogWarning(exception, "Client error: {Message}", exception.Message);
            }
            else if (exception is DbUpdateException)
            {
                _logger.LogError(exception, "Database error: {Message}", exception.Message);
            }
            else
            {
                _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
            }
        }

        private ErrorResponse CreateErrorResponse(Exception exception, HttpContext httpContext)
        {
            var traceId = httpContext.TraceIdentifier;
            var isDevelopment = _environment.IsDevelopment();

            return exception switch
            {
                // Custom business exceptions
                NotFoundException notFoundException => new ErrorResponse
                {
                    Message = notFoundException.Message,
                    StatusCode = (int)HttpStatusCode.NotFound,
                    TraceId = traceId,
                    Details = isDevelopment ? notFoundException.StackTrace : null
                },

                ValidationException validationException => new ErrorResponse
                {
                    Message = validationException.Message,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Errors = validationException.Errors,
                    TraceId = traceId,
                    Details = isDevelopment ? validationException.StackTrace : null
                },

                BadRequestException badRequestException => new ErrorResponse
                {
                    Message = badRequestException.Message,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    TraceId = traceId,
                    Details = isDevelopment ? badRequestException.StackTrace : null
                },

                // Database exceptions
                DbUpdateException dbUpdateException => HandleDatabaseException(dbUpdateException, traceId, isDevelopment),

                // Standard exceptions
                UnauthorizedAccessException => new ErrorResponse
                {
                    Message = "Access denied. You do not have permission to perform this action.",
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    TraceId = traceId
                },

                ArgumentNullException argumentNullException => new ErrorResponse
                {
                    Message = $"Required parameter is missing: {argumentNullException.ParamName}",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    TraceId = traceId,
                    Details = isDevelopment ? argumentNullException.Message : null
                },

                ArgumentException argumentException => new ErrorResponse
                {
                    Message = argumentException.Message,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    TraceId = traceId,
                    Details = isDevelopment ? argumentException.StackTrace : null
                },

                // Fallback for unexpected errors
                _ => new ErrorResponse
                {
                    Message = "An unexpected error occurred. Please try again later.",
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    TraceId = traceId,
                    Details = isDevelopment ? $"{exception.Message}\n{exception.StackTrace}" : null
                }
            };
        }

        private ErrorResponse HandleDatabaseException(DbUpdateException dbException, string traceId, bool isDevelopment)
        {
            var innerException = dbException.InnerException;

            // Handle SQLite specific errors
            if (innerException is SqliteException sqliteException)
            {
                return sqliteException.SqliteErrorCode switch
                {
                    // SQLITE_CONSTRAINT (19) - Constraint violation
                    19 => new ErrorResponse
                    {
                        Message = ParseSqliteConstraintError(sqliteException.Message),
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        TraceId = traceId,
                        Details = isDevelopment ? sqliteException.Message : null
                    },

                    // Other SQLite errors
                    _ => new ErrorResponse
                    {
                        Message = "A database error occurred while processing your request.",
                        StatusCode = (int)HttpStatusCode.InternalServerError,
                        TraceId = traceId,
                        Details = isDevelopment ? sqliteException.Message : null
                    }
                };
            }

            // Generic database error
            return new ErrorResponse
            {
                Message = "A database error occurred while saving your changes.",
                StatusCode = (int)HttpStatusCode.InternalServerError,
                TraceId = traceId,
                Details = isDevelopment ? dbException.Message : null
            };
        }

        private string ParseSqliteConstraintError(string errorMessage)
        {
            if (errorMessage.Contains("FOREIGN KEY constraint failed", StringComparison.OrdinalIgnoreCase))
            {
                return "One or more referenced items do not exist. Please verify that all related data (colours, product types, etc.) exist before creating this record.";
            }

            if (errorMessage.Contains("UNIQUE constraint failed", StringComparison.OrdinalIgnoreCase))
            {
                return "A record with this information already exists. Please use unique values.";
            }

            if (errorMessage.Contains("NOT NULL constraint failed", StringComparison.OrdinalIgnoreCase))
            {
                return "Required field is missing. Please provide all required information.";
            }

            return "Data validation failed. Please check your input and try again.";
        }
    }
}
