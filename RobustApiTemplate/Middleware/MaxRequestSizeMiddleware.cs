using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Controllers;
using RobustApiTemplate.Common;
using RobustApiTemplate.Models;

namespace RobustApiTemplate.Middleware;

public class MaxRequestSizeMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly long _defaultMaxSizeInBytes;

    public MaxRequestSizeMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
        _defaultMaxSizeInBytes =
            _configuration.GetValue<long>("RequestSizeLimits:DefaultMaxSizeInBytes", 1024 * 1024); // 1 MB default
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();

        if (endpoint != null)
        {
            var controllerActionDescriptor =
                endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();

            if (controllerActionDescriptor != null)
            {
                // Build the key for endpoint-specific size limits
                var controllerName = controllerActionDescriptor.ControllerName;
                var actionName = controllerActionDescriptor.ActionName;
                var endpointKey = $"RequestSizeLimits:Endpoints:{controllerName}.{actionName}";

                // Retrieve the max size from configuration, fallback to default if not found
                long maxSizeInBytes = _configuration.GetValue<long?>(endpointKey) ?? _defaultMaxSizeInBytes;

                // Check the request size if the Content-Length header is present
                if (context.Request.ContentLength > maxSizeInBytes)
                {
                    var errorResponse = new ErrorResponse
                    {
                        CorrelationId = context.GetOrSetCorrelationId(),
                        StatusCode = StatusCodes.Status413PayloadTooLarge,
                        Message = "Request size exceeds the allowed limit.",
                        Details = [$"The request size was {context.Request.ContentLength}, which exceeds the allowed limit of {maxSizeInBytes} bytes."]
                    };

                    context.Response.StatusCode = StatusCodes.Status413PayloadTooLarge;
                    context.Response.ContentType = "application/json";

                    var errorResponseJson = JsonSerializer.Serialize(errorResponse);
                    await context.Response.WriteAsync(errorResponseJson);

                    return;
                }
            }
        }

        await _next(context);
    }
}
