using System.Runtime.InteropServices.JavaScript;
using Microsoft.Extensions.Logging;

namespace Logger;

public class Result<TSuccess, TErrorType> where TErrorType : Enum
{
    public TSuccess SuccessValue { get; } = default!;
    public TErrorType ErrorType { get; } = default!;
    public string Message { get; }
    public bool IsSuccess { get; }

    private Result(TSuccess successValue, string message)
    {
        SuccessValue = successValue;
        Message = message;
        IsSuccess = true;
    }

    private Result(TErrorType errorType, string message)
    {
        ErrorType = errorType;
        Message = message;
        IsSuccess = false;
    }

    public static Result<TSuccess, TErrorType> Success(TSuccess value, string message="No message")
    {
        Logger.Instance.GetLogger<Result<TSuccess, TErrorType>>().LogInformation(message);
        return new Result<TSuccess, TErrorType>(value, message);
    }

    public static Result<TSuccess, TErrorType> Fail(TErrorType errorType, string message="No message")
    {
        Logger.Instance.GetLogger<Result<TSuccess, TErrorType>>().LogError(message);
        return new Result<TSuccess, TErrorType>(errorType, message);
    }
}
