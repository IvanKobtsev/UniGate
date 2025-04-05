namespace UniGate.Common.Exceptions;

public class ServiceUnavailableException(string message) : BaseException(message, 503);