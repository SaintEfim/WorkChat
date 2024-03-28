using System.Net;

namespace Service.Authify.API.Infrastructure.ExceptionHandling.Helpers;

public class GetTitleAndStatusCodeHelper
{
    private readonly Dictionary<Type, (string title, int statusCode)> _exceptionMapping;
    private readonly string _defaultTitle;

    public GetTitleAndStatusCodeHelper(Dictionary<Type, (string title, int statusCode)> exceptionMapping,
        IConfiguration config)
    {
        _exceptionMapping = exceptionMapping;
        _defaultTitle = config.GetValue<string>("ErrorSettings:DefaultTitle")!;
    }

    public (string, int) GetTitleAndStatusCode(Exception exception)
    {
        string title;
        int statusCode;

        if (_exceptionMapping.TryGetValue(exception.GetType(), out var mapping))
        {
            title = mapping.title;
            statusCode = mapping.statusCode;
        }
        else
        {
            title = _defaultTitle;
            statusCode = (int)HttpStatusCode.InternalServerError;
        }

        return (title, statusCode);
    }
}
