namespace NordesteFoodAPI.Shared.Infraestructure.Results
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string? ErrorMessage { get; }
        public ErrorType ErrorType { get; }

        protected Result(bool isSuccess, string? errorMessage, ErrorType errorType)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            ErrorType = errorType;
        }

        public static Result Success() => new(true, null,  ErrorType.None);
        public static Result Failure(string error, ErrorType errorType) => new(false, error, errorType);
    }

    public class Result<T> : Result
    {
        public T? Value { get; }

        private Result(bool isSuccess, T? value, string? errorMessage, ErrorType errorType) : base(isSuccess, errorMessage, errorType)
        {
            Value = value;
        }

        public static Result<T> Success(T value) => new(true, value, null, ErrorType.None);
        public static new Result<T> Failure(string errorMessage, ErrorType errorType) => new(false, default, errorMessage, errorType);
    }
}
