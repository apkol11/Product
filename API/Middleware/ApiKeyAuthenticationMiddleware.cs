using Domain.Response;

namespace API.Middleware
{
    /// <summary>
    /// Middleware for API Key authentication.
    /// Validates the X-API-Key header against the configured API key.
    /// </summary>
    public class ApiKeyAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApiKeyAuthenticationMiddleware> _logger;
        private const string API_KEY_HEADER_NAME = "X-API-Key";

        public ApiKeyAuthenticationMiddleware(
            RequestDelegate next,
            IConfiguration configuration,
            ILogger<ApiKeyAuthenticationMiddleware> logger)
        {
            _next = next;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Skip authentication for Swagger endpoints
            if (context.Request.Path.StartsWithSegments("/swagger") ||
                context.Request.Path.StartsWithSegments("/index.html") ||
                context.Request.Path == "/")
            {
                await _next(context);
                return;
            }

            // Check if API key header exists
            if (!context.Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out var extractedApiKey))
            {
                _logger.LogWarning("API Key missing from request: {Path}", context.Request.Path);
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new ErrorResponse
                {
                    Message = "API Key is missing. Please provide X-API-Key header.",
                    StatusCode = 401,
                    TraceId = context.TraceIdentifier,
                    Timestamp = DateTime.UtcNow
                });
                return;
            }

            // Validate API key
            var configuredApiKey = _configuration["ApiKey"];
            if (string.IsNullOrEmpty(configuredApiKey))
            {
                _logger.LogError("API Key not configured in appsettings.json");
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new ErrorResponse
                {
                    Message = "Server configuration error.",
                    StatusCode = 500,
                    TraceId = context.TraceIdentifier,
                    Timestamp = DateTime.UtcNow
                });
                return;
            }

            if (!string.Equals(extractedApiKey, configuredApiKey, StringComparison.Ordinal))
            {
                _logger.LogWarning("Invalid API Key attempt from {IP}: {Path}", 
                    context.Connection.RemoteIpAddress, 
                    context.Request.Path);
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new ErrorResponse
                {
                    Message = "Invalid API Key.",
                    StatusCode = 401,
                    TraceId = context.TraceIdentifier,
                    Timestamp = DateTime.UtcNow
                });
                return;
            }

            // API Key is valid, continue to next middleware
            await _next(context);
        }
    }
}
