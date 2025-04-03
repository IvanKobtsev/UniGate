namespace UniGate.Common.Exceptions;

public abstract class BaseException(string message, int statusCode) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}