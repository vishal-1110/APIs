using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace APIs.Middleware
{
    public class RequestTimingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimingMiddleware> _logger;

        public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            try { 
                await _next(context); 
            }
            finally
            {
                stopwatch.Stop();

                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                _logger.LogInformation(
                    "Request {Method} {Path} executed in {ElapsedMilliseconds} ms with status {StatusCode}",
                    context.Request.Method,
                    context.Request.Path,
                    elapsedMilliseconds,
                    context.Response.StatusCode
                 );
            }
        }
    }
}
