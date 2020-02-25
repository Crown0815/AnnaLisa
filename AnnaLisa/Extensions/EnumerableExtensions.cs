using System;
using System.Collections.Generic;
using System.Linq;

namespace AnnaLisa.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable is null) return true;
            return !enumerable.Any();
        }

        public static TValue Shared<TItem, TValue>(this IEnumerable<TItem> enumerable, Func<TItem, TValue> filter)
        {
            return enumerable.Select(filter).Distinct().Single();
        }
    }
}