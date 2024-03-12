namespace Service.Authify.Domain.Exceptions;

public class NotFoundUserException : Exception
{
    public NotFoundUserException(string message) : base(message)
    {
    }
}