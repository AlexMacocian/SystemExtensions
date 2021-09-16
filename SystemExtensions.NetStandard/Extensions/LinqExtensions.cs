using System.Collections.Generic;
using System.Linq;

namespace System.Extensions
{
    public static class LinqExtensions
    {
        public static bool None<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            enumerable.ThrowIfNull(nameof(enumerable));
            predicate.ThrowIfNull(nameof(predicate));

            return !enumerable.Any(predicate);
        }

        public static bool None<T>(this IEnumerable<T> enumerable)
        {
            enumerable.ThrowIfNull(nameof(enumerable));

            return !enumerable.Any();
        }

        public static ICollection<T> ClearAnd<T>(this ICollection<T> collection)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            collection.Clear();
            return collection;
        }
        
        public static ICollection<T> AddRange<T>(this ICollection<T> collection, IEnumerable<T> values)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            foreach (var item in values)
            {
                collection.Add(item);
            }

            return collection;
        }

        public static int IndexOfWhere<T>(this ICollection<T> collection, Func<T, bool> selector)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (selector is null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            var index = 0;
            foreach (var item in collection)
            {
                if (selector(item))
                {
                    return index;
                }
                index++;
            }

            return -1;
        }

        public static IEnumerable<T> Do<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach(var value in enumerable)
            {
                action(value);
                yield return value;
            }
        }
    }
}
