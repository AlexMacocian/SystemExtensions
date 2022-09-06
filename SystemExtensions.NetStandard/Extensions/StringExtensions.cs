using System.Text;

namespace System.Extensions;

public static class StringExtensions
{
    public static bool IsNullOrEmpty(this string s)
    {
        return string.IsNullOrEmpty(s);
    }

    public static bool IsNullOrWhiteSpace(this string s)
    {
        return string.IsNullOrWhiteSpace(s);
    }

    public static byte[] GetBytes(this string s)
    {
        return Encoding.UTF8.GetBytes(s);
    }

    public static string GetString(this byte[] bytes)
    {
        return Encoding.UTF8.GetString(bytes);
    }
}
