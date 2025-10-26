namespace Course4.Services
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; }
        public T? Data { get; }
        public string? ErrorMessage { get; }
        public int StatusCode { get; }
        private OperationResult(T? data, bool isSuccess, string? errorMessage, int statusCode)
        {
            Data = data;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }  
        public static OperationResult<T> Success(T data)
        {
            return new OperationResult<T>(data, true, null, 200);
        }        
        public static OperationResult<T> ValidationError(string errorMessage)
        {
            return new OperationResult<T>(default, false, errorMessage, 400);
        }       
        public static OperationResult<T> NotFound(string errorMessage)
        {
            return new OperationResult<T>(default, false, errorMessage, 404);
        }        
        public static OperationResult<T> ServerError(string errorMessage)
        {
            return new OperationResult<T>(default, false, errorMessage, 500);
        }
    }    
    public class OperationResult
    {
        public bool IsSuccess { get; }
        public string? ErrorMessage { get; }
        public int StatusCode { get; }
        public bool IsValid { get; internal set; }
        private OperationResult(bool isSuccess, string? errorMessage, int statusCode)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }
        public static OperationResult Success()
        {
            return new OperationResult(true, null, 200);
        }
        public static OperationResult ValidationError(string errorMessage)
        {
            return new OperationResult(false, errorMessage, 400);
        }
        public static OperationResult NotFound(string errorMessage)
        {
            return new OperationResult(false, errorMessage, 404);
        }
        public static OperationResult ServerError(string errorMessage)
        {
            return new OperationResult(false, errorMessage, 500);
        }
    }
}