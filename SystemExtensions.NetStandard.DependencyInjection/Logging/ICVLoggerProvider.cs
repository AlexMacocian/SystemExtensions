using Microsoft.Extensions.Logging;

namespace System.Logging
{
    public interface ICVLoggerProvider : ILoggerProvider
    {
        void LogEntry(Log log);
    }
}
