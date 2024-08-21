using Microsoft.Extensions.Logging;
using System.Logging;

namespace System.Extensions;

public static class LoggingExtensions
{
    public static ScopedLogger<T> CreateScopedLogger<T>(this ILogger<T> logger, string methodName, string flowIdentifier)
    {
        return ScopedLogger<T>.Create(logger, methodName, flowIdentifier);
    }

    public static ScopedLogger<T> CreateScopedLogger<T>(this ILogger<T> logger, string methodName)
    {
        return ScopedLogger<T>.Create(logger, methodName, string.Empty);
    }
}