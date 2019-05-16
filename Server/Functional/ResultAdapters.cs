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

        /// <summary>
        /// Combine two results into one that will contain the success values of both results.
        /// </summary>
        /// <typeparam name="T1">The tpye of the first result value.</typeparam>
        /// <typeparam name="T2">The type of the second result value.</typeparam>
        /// <param name="result1">The first result.</param>
        /// <param name="result2">The second result.</param>
        /// <returns>A result that contains the success values of both results, or the error of any failed result.</returns>
        public static Result<(T1, T2)> And<T1, T2>(this Result<T1> result1, Result<T2> result2) =>
            result1.And(result2, (success1, success2) => (success1, success2), (error1, error2) => new ErrorMessage(error1 + "; " + error2));

        /// <summary>
        /// Combine two results into one, using the provided methods for combining success and error results.
        /// If both results are a success then the addSuccess method will be used to combine the successes.
        /// If both results are failures then the addError method will be used to combine the errors.
        /// Otherwise if only one result is a failure, then its error will be returned.
        /// </summary>
        /// <typeparam name="T1">The tpye of the first result value.</typeparam>
        /// <typeparam name="T2">The type of the second result value.</typeparam>
        /// <typeparam name="TResult">The type of the output result value.</typeparam>
        /// <param name="result1">The first result.</param>
        /// <param name="result2">The second result.</param>
        /// <param name="addSuccess">The function to combine the successes, if both results are successes.</param>
        /// <param name="addError">The function to combine the errors, if both results are failures.</param>
        /// <returns>A result that contains the combined input results.</returns>
        public static Result<TResult> And<T1, T2, TResult>(this Result<T1> result1, Result<T2> result2, Func<T1, T2, TResult> addSuccess, Func<ErrorMessage, ErrorMessage, ErrorMessage> addError)
        {
            if      ( result1.IsSuccess &&  result2.IsSuccess)   return addSuccess((Success<T1>)result1, (Success<T2>)result2);
            else if ( result1.IsSuccess && !result2.IsSuccess)   return new Error<TResult>((Error<T2>)result2);
            else if (!result1.IsSuccess &&  result2.IsSuccess)   return new Error<TResult>((Error<T1>)result1);
            else  /*(!result1.IsSuccess && !result2.IsSuccess)*/ return addError((Error<T1>)result1, (Error<T2>)result2);
        }
    }
}
