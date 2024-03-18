namespace Service.Authify.Domain.Exceptions;

public class HashVerificationException : Exception
{
    public HashVerificationException(string message)
        : base(message)
    {
    }

    public HashVerificationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}