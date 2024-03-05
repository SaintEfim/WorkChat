namespace Service.Authify.Data.Exceptions;

public class InvalidTokenException : Exception
{
    public InvalidTokenException(string message) : base(message)
    {
    }
}