namespace ProductAPI.Models;

public abstract class ApiResponse<T>
{
    public bool IsSuccess { get; }
    public T? Data { get; }
    public string? ErrorMessage { get; }

    protected ApiResponse(bool isSuccess, T? data, string? errorMessage)
    {
        IsSuccess = isSuccess;
        Data = data;
        ErrorMessage = errorMessage;
    }
}

public class SuccessResponse<T> : ApiResponse<T>
{
    public SuccessResponse(T data) : base(true, data, null) { }
}

public class FailureResponse<T> : ApiResponse<T>
{
    public FailureResponse(string errorMessage) : base(false, default, errorMessage) { }
}
