using Microsoft.Extensions.Logging;

namespace System.Logging;

public sealed record Log
{
    public Exception Exception { get; set; }
    public DateTime LogTime { get; set; }
    public string Category { get; set; }
    public string EventId { get; set; }
    public LogLevel LogLevel { get; set; }
    public string Message { get; set; }
    public string CorrelationVector { get; set; }
}