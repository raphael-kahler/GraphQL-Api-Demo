using System;

namespace Functional
{
    public static class ResultAdapters
    {
        /// <summary>
        /// Perform an action on the result content if the result is a success.
        /// </summary>
        /// <typeparam name="T">The type of the result content.</typeparam>
        /// <param name="result">The result.</param>
        /// <param name="action">The action to perform if the result is a success.</param>
        /// <returns>The original result.</returns>
        public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> action)
        {
            if (result is Success<T> success) action(success);
            return result;
        }

        /// <summary>
        /// Perform an action on the result content if the result is an error.
        /// </summary>
        /// <typeparam name="T">The type of the result content.</typeparam>
        /// <param name="result">The result.</param>
        /// <param name="action">The action to perform if the result is an error.</param>
        /// <returns>The original result.</returns>
        public static Result<T> OnFailure<T>(this Result<T> result, Action<ErrorMessage> action)
        {
            if (result is Error<T> success) action(success);
            return result;
        }

        /// <summary>
        /// Map a result to a new value if it is a success. Otherwise if the result if a failure, retain the failure.
        /// </summary>
        /// <typeparam name="T">The type of the result value.</typeparam>
        /// <typeparam name="TResult">The new type of the result.</typeparam>
        /// <param name="result">The result.</param>
        /// <param name="map">The function to map the value to the new value, if the result is a success.</param>
        /// <returns>The mapped result.</returns>
        public static Result<TResult> Map<T, TResult>(this Result<T> result, Func<T, Result<TResult>> map) =>
            result is Success<T> success ? map(success) : new Error<TResult>((Error<T>)result);

        /// <summary>
        /// Reduce a result to its value if it is a success. Otherwise if it is an error, return the specified value instead.
        /// </summary>
        /// <typeparam name="T">The type of the result value.</typeparam>
        /// <param name="result">The result.</param>
        /// <param name="whenError">The value to use if the result is an error.</param>
        /// <returns>The result value if it is a success, or the specified value if the result is an error.</returns>
        public static T Reduce<T>(this Result<T> result, T whenError) =>
            result is Success<T> success ? success : whenError;

        /// <summary>
        /// Reduce a result to its value if it is a success. Otherwise if it is an error, use the provided function to handle the error and return an alternative value.
        /// </summary>
        /// <typeparam name="T">The type of the result value.</typeparam>
        /// <param name="result">The result.</param>
        /// <param name="handleError">The function to handle the error and return an alternative value.</param>
        /// <returns>The result value if it is a success, or the return value of the error handle function.</returns>
        public static T Reduce<T>(this Result<T> result, Func<ErrorMessage, T> handleError) =>
            result is Success<T> success ? (T)success : handleError((Error<T>)result);
    }
}
