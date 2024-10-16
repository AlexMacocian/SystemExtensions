using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System.Core.Extensions;

public static class ObjectExtensions
{
    public static T ThrowIfNull<T>([NotNull] this T? obj, [CallerArgumentExpression("obj")] string? paramName = null)
        where T : class
    {
        if (obj is null)
        {
            throw new ArgumentNullException(paramName);
        }

        return obj;
    }
}
