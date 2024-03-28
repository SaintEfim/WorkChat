using System.Net;

namespace Service.Authify.Domain.Exceptions;

public class DuplicateUserException : Exception
{
    public DuplicateUserException()
    {
    }

    public DuplicateUserException(string message) : base(message)
    {
    }

    public string Title = "Duplicate User";
    public int StatusCode = (int)HttpStatusCode.Conflict;
}