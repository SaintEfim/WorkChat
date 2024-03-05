namespace Service.Authify.Data.Exceptions;

public class NotFoundUserException : Exception
{
    public NotFoundUserException(string message) : base(message)
    {
    }
}