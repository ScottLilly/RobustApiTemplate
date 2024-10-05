namespace RobustApiTemplate.Models;

public class ErrorResponse
{
    public string CorrelationId { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Details { get; set; } = [];
}
