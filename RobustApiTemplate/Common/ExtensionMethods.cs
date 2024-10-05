namespace RobustApiTemplate.Common;

public static class ExtensionMethods
{
    public static string GetOrSetCorrelationId(this HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(Constants.CORRELATION_ID_HEADER, out var correlationId))
        {
            if (!string.IsNullOrEmpty(correlationId))
            {
                return correlationId.ToString();
            }
        }

        // Optionally, generate a new correlation ID if not present
        var newCorrelationId = Guid.NewGuid().ToString();
        context.Request.Headers[Constants.CORRELATION_ID_HEADER] = newCorrelationId;
        return newCorrelationId;
    }
}
