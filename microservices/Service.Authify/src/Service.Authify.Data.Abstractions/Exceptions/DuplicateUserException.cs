namespace Service.Authify.Data.Exceptions;

public class DuplicateUserException : Exception
{
    public DuplicateUserException(string message) : base(message)
    {
    }
}