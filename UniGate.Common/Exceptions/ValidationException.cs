namespace UniGate.Common.Exceptions;

public class ValidationException(string message) : BaseException(message, 400);