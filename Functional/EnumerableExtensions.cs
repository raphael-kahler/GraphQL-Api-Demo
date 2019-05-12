using System.Collections.Generic;
using System.Linq;

namespace Functional
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Flatten a collection of optional values and retain only those values that are some, and remove the values that are none.
        /// </summary>
        /// <typeparam name="T">The type of the optional value.</typeparam>
        /// <param name="options">The collection of optional values.</param>
        /// <returns>A collection of the values from the input collection that are not none.</returns>
        public static IEnumerable<T> Flatten<T>(this IEnumerable<Option<T>> options) =>
            options
                .OfType<Some<T>>()
                .Select(o => (T)o);
    }
}
