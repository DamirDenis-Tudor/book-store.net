/**************************************************************************
 *                                                                        *
 *  Description: Functional Programming Monad Implementation              *
 *  Website:     https://github.com/DamirDenis-Tudor/PetShop-ProiectIP    *
 *  Copyright:   (c) 2024, Damir Denis-Tudor                              *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/

namespace Common;

/// <summary>
/// Class representing Void type.
/// Usefully when the result is not important.
/// </summary>
public class VoidResult
{
    public static VoidResult Get() => new ();
}

/// <summary>
/// Generic class representing a result of an operation.
/// </summary>
/// <typeparam name="TSuccess">The type of the success value.</typeparam>
/// <typeparam name="TErrorType">The type of the error.</typeparam>
public class Result<TSuccess, TErrorType> where TErrorType : Enum
{
    /// <summary>
    /// The success value.
    /// </summary>
    public TSuccess SuccessValue { get; } = default!;

    /// <summary>
    /// The error type.
    /// </summary>
    public TErrorType ErrorType { get; } = default!;

    /// <summary>
    /// The message associated with the result.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
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
        Message = $"[{errorType}]: {message}";
        IsSuccess = false;
    }

    /// <summary>
    /// Creates a success result.
    /// </summary>
    /// <param name="value">The success value.</param>
    /// <param name="message">The message associated with the result.</param>
    /// <returns>A success result.</returns>
    public static Result<TSuccess, TErrorType> Success(TSuccess value, string message = "No message")
    {
        return new Result<TSuccess, TErrorType>(value, message);
    }

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="errorType">The error type.</param>
    /// <param name="message">The message associated with the result.</param>
    /// <returns>A failure result.</returns>
    public static Result<TSuccess, TErrorType> Fail(TErrorType errorType, string message = "No message")
    {
        return new Result<TSuccess, TErrorType>(errorType, message);
    }
}