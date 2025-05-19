using InRoom.BLL.Interfaces;

namespace InRoom.API.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IRequestLogService logService)
    {
        if (context.Request.ContentLength > 0 && context.Request.Body.CanSeek)
        {
            context.Request.EnableBuffering();
            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }

        await _next(context);

        var requestLogService = context.RequestServices.GetRequiredService<IRequestLogService>();
        await requestLogService.LogAsync(context.Request, context.Response);
    }
}