using System;

namespace Functional
{
    public static class OptionAdapters
    {
        /// <summary>
        /// Map an option to a new value if it is some value. Otherwise if the option is none, do nothing.
        /// </summary>
        /// <typeparam name="T">The type of the option value.</typeparam>
        /// <typeparam name="TResult">The result type of the option value.</typeparam>
        /// <param name="option">The optional value.</param>
        /// <param name="map">The function to map the value to its result.</param>
        /// <returns>The mapped optional value if it was some, or none if it was none.</returns>
        public static Option<TResult> Map<T, TResult>(this Option<T> option, Func<T, TResult> map) =>
            option is Some<T> some ? map(some) : (Option<TResult>)None.Value;

        /// <summary>
        /// Turn a value into an option so that it is some if it meets the predicate and none if it doesn't.
        /// </summary>
        /// <typeparam name="T">The type of the option value.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="predicate">The predicate that returns true if the value is some and false if the value is none.</param>
        /// <returns>An optional value.</returns>
        public static Option<T> When<T>(this T value, Func<T, bool> predicate) =>
            predicate(value) ? value : (Option<T>)None.Value;

        /// <summary>
        /// Reduce an option to its value if it is some. Otherwise if it is none, return the specified value instead.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="option">The optional value.</param>
        /// <param name="whenNone">The value to return if the option is none.</param>
        /// <returns>The option value if it is some, or the specified value if the option is none.</returns>
        public static T Reduce<T>(this Option<T> option, T whenNone) =>
            option is Some<T> some ? some : whenNone;

        /// <summary>
        /// Reduce an option to its value if it is some. Otherwise if it is none, apply a function that produces a value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="option">The optional value.</param>
        /// <param name="whenNone">The function to execute if the option is none.</param>
        /// <returns>The option value or the result of the function.</returns>
        public static T Reduce<T>(this Option<T> option, Func<T> whenNone) =>
            option is Some<T> some ? some : whenNone();

    }
}
