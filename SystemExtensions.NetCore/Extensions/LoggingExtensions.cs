using Microsoft.Extensions.Logging;
using System.Logging;
using System.Runtime.CompilerServices;

namespace System.Extensions.Core;
public static class LoggingExtensions
{
    public static ScopedLogger<T> CreateScopedLogger<T>(this ILogger<T> logger, string flowIdentifier, [CallerMemberName] string? methodName = default)
    {
        return ScopedLogger<T>.Create(logger, methodName ?? string.Empty, flowIdentifier);
    }

    public static ScopedLogger<T> CreateScopedLogger<T>(this ILogger<T> logger, [CallerMemberName] string? methodName = default)
    {
        return ScopedLogger<T>.Create(logger, methodName ?? string.Empty, string.Empty);
    }
}
