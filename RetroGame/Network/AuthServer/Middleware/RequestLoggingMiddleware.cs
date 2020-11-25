using AuthServer.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AuthServer.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
            _logger = LoggingUtils.CreateLogger<RequestLoggingMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            } finally
            {
                _logger.LogInformation($"Request " +
                    $"{context.Request?.Method} " +
                    $"{context.Request?.Path.Value} " +
                    $"=> {context.Response?.StatusCode}");
            }
        }
    }
}
