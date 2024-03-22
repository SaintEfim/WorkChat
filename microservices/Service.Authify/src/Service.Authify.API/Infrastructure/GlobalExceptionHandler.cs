using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Service.Authify.API.Models;
using Service.Authify.Domain.Exceptions;

namespace Service.Authify.API.Infrastructure;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    private readonly Dictionary<Type, (string title, int statusCode)> _exceptionMapping =
        new()
        {
            { typeof(AuthenticationFailedException), ("Authentication Failed", (int)HttpStatusCode.Unauthorized) },
            { typeof(ClaimsGenerationException), ("Claims Generation Error", (int)HttpStatusCode.InternalServerError) },
            { typeof(DataNotFoundException), ("Data Not Found", (int)HttpStatusCode.NotFound) },
            { typeof(DuplicateUserException), ("Duplicate User", (int)HttpStatusCode.Conflict) },
            { typeof(HashVerificationException), ("Hash Verification Error", (int)HttpStatusCode.InternalServerError) },
            { typeof(InvalidTokenException), ("Invalid Token", (int)HttpStatusCode.BadRequest) },
            { typeof(KeyGenerationException), ("Key Generation Error", (int)HttpStatusCode.InternalServerError) },
            { typeof(NotFoundUserException), ("User Not Found", (int)HttpStatusCode.NotFound) },
            { typeof(PasswordMismatchException), ("Password Mismatch", (int)HttpStatusCode.BadRequest) },
        };

    private const string ErrorType = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1";

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, exception.Message);

        var (title, statusCode) = GetTitleAndStatusCode(exception);

        var details = BuildErrorDto(title, statusCode, exception);
        var response = JsonSerializer.Serialize(details);
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsync(response, cancellationToken);

        return true;
    }

    private ErrorDto BuildErrorDto(string title, int statusCode, Exception exception)
    {
        var errors = new Dictionary<string, string[]> { { exception.GetType().Name, [exception.Message] } };

        return new ErrorDto
        {
            Type = ErrorType,
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
            title = "One or more validation errors occurred.";
            statusCode = (int)HttpStatusCode.InternalServerError;
        }

        return (title, statusCode);
    }
}