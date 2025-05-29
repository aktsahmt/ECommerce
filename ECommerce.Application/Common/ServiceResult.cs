namespace ECommerce.Application.Common;

public class ServiceResult<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public string? Error { get; set; }

    public static ServiceResult<T> Ok(T data, string? message = null)
        => new() { Success = true, Data = data, Message = message };

    public static ServiceResult<T> Fail(string message, string? error = null)
        => new() { Success = false, Error = error, Message = message };
}