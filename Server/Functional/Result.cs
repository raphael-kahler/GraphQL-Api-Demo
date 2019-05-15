namespace Functional
{
    public class Result<T>
    {
        public bool IsSuccess => this is Success<T>;

        public static implicit operator Result<T>(T content) => new Success<T>(content);
        public static implicit operator Result<T>(ErrorMessage error) => new Error<T>(error);
    }

    public class Success<T> : Result<T>
    {
        private T Content { get; }

        public Success(T content)
        {
            Content = content;
        }

        public static implicit operator T(Success<T> success) => success.Content;
    }

    public class Error<T> : Result<T>
    {
        private ErrorMessage ErrorMessage { get; }

        public Error(ErrorMessage errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public static implicit operator ErrorMessage(Error<T> error) => error.ErrorMessage;
   }

    public class ErrorMessage
    {
        public string Message { get; }

        public ErrorMessage(string message)
        {
            Message = message;
        }

        public static implicit operator string(ErrorMessage errorMessage) => errorMessage.Message;
    }
}
