namespace Service.Authify.Domain.Exceptions;

public class MappingFailedException : Exception
{
    public MappingFailedException(string message) : base(message)
    {
    }
}