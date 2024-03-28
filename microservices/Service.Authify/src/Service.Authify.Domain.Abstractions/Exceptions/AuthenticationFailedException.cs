using System.Net;

namespace Service.Authify.Domain.Exceptions;

public class AuthenticationFailedException : Exception
{
    public AuthenticationFailedException()
    {
    }

    public AuthenticationFailedException(string message) : base(message)
    {
    }

    public string Title = "Authentication Failed";
    public int StatusCode = (int)HttpStatusCode.Unauthorized;
}