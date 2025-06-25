using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ProChess.Server.Exceptions;

public class UnauthorizedExceptionHandler: IExceptionHandler
{
    private readonly ILogger<UnauthorizedExceptionHandler> _logger;

    public UnauthorizedExceptionHandler(ILogger<UnauthorizedExceptionHandler> logger)
    {
        _logger = logger;
    }
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is UnauthorizedException)
        {
            _logger.LogError(exception,"Exception occured: {Message}", exception.Message);

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Detail = exception.Message
            };
            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
        return false;
    }
}