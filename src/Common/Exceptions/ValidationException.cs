namespace Common.Exceptions;

public class ValidationException : BusinessException
{
    public ValidationException(string message)
        : base(message, "VALIDATION_ERROR", 400)
    {
    }

    public ValidationException(string message, Exception innerException)
        : base(message, innerException, "VALIDATION_ERROR", 400)
    {
    }
}

