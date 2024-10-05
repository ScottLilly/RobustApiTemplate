using RobustApiTemplate.Common;

namespace RobustApiTemplate.Middleware;

/// <summary>
/// Middleware to generate a CorrelationId for each request.
/// This CorrelationId is used to track a request through the system.
/// </summary>
/// <param name="next">Next RequestDelegate middleware</param>
public class CorrelationIdMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        // This will add a CorrelationId to the request headers if not present
        context.GetOrSetCorrelationId();

        // Proceed with the next middleware
        await _next(context);
    }
}
