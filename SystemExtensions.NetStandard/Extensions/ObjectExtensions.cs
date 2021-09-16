using Newtonsoft.Json;

namespace System.Extensions
{
    public static class ObjectExtensions
    {
        public static T Deserialize<T>(this string serialized)
            where T : class
        {
            if (serialized.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("Provided serialized string cannot be null or whitespace", nameof(serialized));
            }

            return JsonConvert.DeserializeObject<T>(serialized);
        }

        public static string Serialize<T>(this T obj)
            where T : class
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return JsonConvert.SerializeObject(obj);
        }

        public static T Cast<T>(this object obj)
        {
            return (T)obj;
        }

        public static T As<T>(this object obj) where T : class
        {
            return obj as T;
        }

        public static Optional<T> ToOptional<T>(this T obj)
        {
            return Optional.FromValue(obj);
        }

        public static T ThrowIfNull<T>([ValidatedNotNull] this T obj, string name) where T : class
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
}
