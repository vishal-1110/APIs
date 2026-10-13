using System.Diagnostics;

namespace APIs.Middleware
{
    public class RequestExecutionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestExecutionMiddleware> _logger;

        public RequestExecutionMiddleware(RequestDelegate next, ILogger<RequestExecutionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var timer = Stopwatch.StartNew();

            // Reading incoming header values
            var clientTraceId = context.Request.Headers["X-Client-Trace-Id"].FirstOrDefault() ?? "None";
            _logger.LogInformation("--> Incoming Request: {Method} {Path} | TraceId: {TraceId}",
                context.Request.Method, context.Request.Path, clientTraceId);

            // Forward execution downstream
            await _next(context);

            // Code runs on the way back out
            timer.Stop();
            _logger.LogInformation("<-- Completed: {StatusCode} in {Elapsed} ms",
                context.Response.StatusCode, timer.ElapsedMilliseconds);
        }
    }
}
