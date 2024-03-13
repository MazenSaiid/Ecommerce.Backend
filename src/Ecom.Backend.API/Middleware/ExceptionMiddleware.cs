using Ecom.Backend.API.Errors;
using System.Net;
using System.Text.Json;

namespace Ecom.Backend.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
                _logger.LogInformation("Successfull request");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"This error is from ExceptionMiddleware {ex.Message}");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var response = new ExceptionError((int)HttpStatusCode.InternalServerError, 
                    ex.Message, ex.StackTrace.ToString());
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response,options);
                await context.Response.WriteAsync(json);
                
            }
        }
    }
}
