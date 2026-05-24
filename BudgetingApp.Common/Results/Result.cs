using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetingApp.Common.Results;

public class Result
{
    public bool IsSuccess { get; }
    public string ErrorMessage { get; } = string.Empty;
    public Exception? Exception { get; }
    public ResultErrorCode ErrorCode { get; }
    public string? AdditionalInformation { get; }

    protected Result(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }

    protected Result(
        bool isSuccess,
        string errorMessage,
        Exception? exception = null,
        ResultErrorCode errorCode = ResultErrorCode.None,
        string? additionalInformation = null)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        Exception = exception;
        ErrorCode = errorCode;
        AdditionalInformation = additionalInformation;
    }

    public static Result Success() => new(true);

    public static Result Failure(
        string errorMessage,
        Exception? exception = null,
        ResultErrorCode errorCode = ResultErrorCode.Unknown,
        string? additionalInformation = null) =>
        new(false, errorMessage, exception, errorCode, additionalInformation);
}

public class Result<T> : Result
{
    public T? Value { get; }

    private Result(bool isSuccess, T? value)
        : base (isSuccess)
    {
        Value = value;
    }

    private Result(
        bool isSuccess,
        string errorMessage,
        Exception? exception = null,
        ResultErrorCode errorCode = ResultErrorCode.None,
        string? additionalInformation = null)
        : base(isSuccess, errorMessage, exception, errorCode, additionalInformation)
    {
        Value = default;
    }

    public static Result<T> Success(T value) =>
        new(true, value);

    public static new Result<T> Failure(
        string errorMessage,
        Exception? exception = null,
        ResultErrorCode errorCode = ResultErrorCode.Unknown,
        string? additionalInformation = null) =>
        new(false, errorMessage, exception, errorCode, additionalInformation);
}
