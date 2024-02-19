namespace Service.Authify.Data.Exceptions;

public class ClaimsGenerationException : Exception
{
    public ClaimsGenerationException(string message) : base(message)
    {
    }
}