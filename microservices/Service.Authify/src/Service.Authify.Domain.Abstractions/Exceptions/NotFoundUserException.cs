using System.Net;

namespace Service.Authify.Domain.Exceptions;

public class NotFoundUserException : Exception
{
    public NotFoundUserException()
    {
    }

    public NotFoundUserException(string message) : base(message)
    {
    }

    public string Title = "User Not Found";
    public int StatusCode = (int)HttpStatusCode.NotFound;
}