namespace Service.Authify.Domain.Exceptions;

public class ClaimsGenerationException : Exception
{
    public ClaimsGenerationException(string message) : base(message)
    {
    }
}