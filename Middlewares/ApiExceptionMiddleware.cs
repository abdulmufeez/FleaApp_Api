using System.Net;
using System.Text.Json;
using FleaApp_Api.Errors;

namespace FleaApp_Api.Middlewares
{
    public class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ApiExceptionMiddleware(RequestDelegate next,
            ILogger<ApiExceptionMiddleware> logger, 
            IHostEnvironment env)
        {        
            _next = next;
            _logger = logger;
            _env = env;
        }

        //creating error middleware
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                    ? new ApiExceptionResponse(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new ApiExceptionResponse(context.Response.StatusCode, "Internal Server Error",null);

                //sending back response in json format
                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
                var jsonResponse = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}