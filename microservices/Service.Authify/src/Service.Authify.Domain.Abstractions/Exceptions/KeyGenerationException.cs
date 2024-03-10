namespace Service.Authify.Domain.Exceptions;

public class KeyGenerationException : Exception
{
    public KeyGenerationException(string message) : base(message)
    {
    }
}