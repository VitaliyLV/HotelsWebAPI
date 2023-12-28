using HotelsApplication.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace HotelsApplication.Middleware
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occured in {context.Request.Path}.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode code = HttpStatusCode.InternalServerError;
            var errorDetails = new ErrorDetails
            {
                Type = "Failure",
                Message = ex.Message
            };
            switch (ex)
            {
                case NotFoundException:
                    code = HttpStatusCode.NotFound;
                    errorDetails.Type = "Not Found";
                    break;
                case DifferentIDsException:
                    code = HttpStatusCode.BadRequest;
                    errorDetails.Type = "IDs don't match";
                    break;
                default: break;
            }
            string response = JsonConvert.SerializeObject(errorDetails);
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(response);
        }
    }
    public class ErrorDetails
    {
        public string Message { get; set; }
        public string Type { get; set; }
    }
}
