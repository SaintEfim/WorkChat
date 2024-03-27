using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Service.Authify.API.Models;

namespace Service.Authify.API.Infrastructure;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    private readonly Dictionary<Type, (string title, int statusCode)> _exceptionMapping;

    private readonly string _errorType;

    private readonly string _defaultTitle;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger,
        Dictionary<Type, (string title, int statusCode)> exceptionMapping, IConfiguration config)
    {
        _logger = logger;
        _exceptionMapping = exceptionMapping;
        _defaultTitle = config.GetValue<string>("ErrorSettings:DefaultTitle")!;
        _errorType = config.GetValue<string>("ErrorSettings:ErrorURLType")!;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, exception.Message);

        var (title, statusCode) = GetTitleAndStatusCode(exception);

        var details = BuildErrorDto(title, statusCode, exception);
        var response = JsonSerializer.Serialize(details);
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = details.Status;

        await httpContext.Response.WriteAsync(response, cancellationToken);

        return true;
    }

    private ErrorDto BuildErrorDto(string title, int statusCode, Exception exception)
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

    private (string, int) GetTitleAndStatusCode(Exception exception)
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