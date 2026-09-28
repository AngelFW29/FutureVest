namespace FutureVest.Application.Common
{
    public class ServiceResponse
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public ErrorType ErrorType { get; set; } = ErrorType.None;

        public static ServiceResponse Ok() => new() { Success = true };
        public static ServiceResponse Fail(string message) => new() { Success = false, ErrorMessage = message };

        public static ServiceResponse Fail(string message, ErrorType errorType) => new()
        {
            Success = false,
            ErrorMessage = message,
            ErrorType = errorType
        };
    }

    public class ServiceResponse<T> : ServiceResponse
    {
        public T? Data { get; set; }

        public static ServiceResponse<T> Ok(T data) => new() { Success = true, Data = data };
        public static new ServiceResponse<T> Fail(string message) => new() { Success = false, ErrorMessage = message };
        public static new ServiceResponse<T> Fail(string message, ErrorType errorType) => new()
        {
            Success = false,
            ErrorMessage = message,
            ErrorType = errorType
        };
    }
}