namespace System.Extensions;

public static class ObjectExtensions
{
    public static T Cast<T>(this object obj)
    {
        return (T)obj;
    }

    public static T? As<T>(this object obj) where T : class
    {
        return obj as T;
    }

    public static T ThrowIfNull<T>([ValidatedNotNull] this T? obj, string name) where T : class
    {
        if (obj is null)
        {
            throw new ArgumentNullException(name);
        }

        return obj;
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    sealed class ValidatedNotNullAttribute : Attribute
    {
    }
}
