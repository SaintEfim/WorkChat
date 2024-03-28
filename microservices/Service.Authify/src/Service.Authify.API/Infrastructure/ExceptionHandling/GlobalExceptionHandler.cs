using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Service.Authify.API.Infrastructure.ExceptionHandling.Helpers;

namespace Service.Authify.API.Infrastructure.ExceptionHandling;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    private readonly BuildErrorDtoHelper _buildErrorDto;

    private readonly GetTitleAndStatusCodeHelper _getTitleAndStatusCode;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, BuildErrorDtoHelper buildErrorDto,
        GetTitleAndStatusCodeHelper getTitleAndStatusCode)
    {
        _logger = logger;
        _buildErrorDto = buildErrorDto;
        _getTitleAndStatusCode = getTitleAndStatusCode;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An error has occurred: {Message}", exception.Message);

        var (title, statusCode) = _getTitleAndStatusCode.GetTitleAndStatusCode(exception);

        var details = _buildErrorDto.BuildErrorDto(title, statusCode, exception);
        var response = JsonSerializer.Serialize(details);
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = details.Status;

        await httpContext.Response.WriteAsync(response, cancellationToken);

        return true;
    }
}