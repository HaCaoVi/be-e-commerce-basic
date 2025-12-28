using System.Net;
using System.Text.Json;
using e_commerce_basic.Common;

namespace e_commerce_basic.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.Clear();
                context.Response.StatusCode = ex switch
                {
                    UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,
                    ArgumentException => (int)HttpStatusCode.BadRequest,
                    BadRequestException => (int)HttpStatusCode.BadRequest,
                    _ => (int)HttpStatusCode.InternalServerError
                };

                context.Response.ContentType = "application/json";

                object? errors = null;
                if (_env.IsDevelopment())
                {
                    errors = new
                    {
                        ex.Message
                    };
                }

                var response = ApiResponse<object>.Fail(
                    ex is UnauthorizedAccessException or ArgumentException or BadRequestException
                        ? ex.Message
                        : "Internal server error",
                    errors
                );

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(
                        response,
                        new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        }
                    )
                );
            }
        }
    }
}
