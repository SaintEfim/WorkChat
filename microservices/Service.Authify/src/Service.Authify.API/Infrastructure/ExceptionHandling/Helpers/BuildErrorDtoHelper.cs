using Service.Authify.API.Models;

namespace Service.Authify.API.Infrastructure.ExceptionHandling.Helpers;

public class BuildErrorDtoHelper
{
    private readonly string _errorType;

    public BuildErrorDtoHelper(IConfiguration config)
    {
        _errorType = config.GetValue<string>("ErrorSettings:ErrorURLType")!;
    }

    public ErrorDto BuildErrorDto(string title, int statusCode, Exception exception)
    {
        var errors = new Dictionary<string, string[]> { { exception.GetType().Name, [exception.Message] } };

        return new ErrorDto
        {
            Type = _errorType,
            Title = title,
            Status = statusCode,
            Errors = errors
        };
    }
}