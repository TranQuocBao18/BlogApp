using System;

namespace Blog.Utilities.Extensions;

public static class CollectionExtension
{
    public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }

        public static void AddRange<T>(this ICollection<T> target, IEnumerable<T> source)
        {
            // Check Condition
            Preconditions.CheckNotNull(target);
            Preconditions.CheckNotNull(source);

            // Execute add element of source to target
            foreach (var element in source)
            {
                target.Add(element);
            }

        }
}
