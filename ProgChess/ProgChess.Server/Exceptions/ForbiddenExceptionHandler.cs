using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ProChess.Server.Exceptions;

public class ForbiddenExceptionHandler: IExceptionHandler
{
    private ILogger<ForbiddenExceptionHandler> _logger;

    public ForbiddenExceptionHandler(ILogger<ForbiddenExceptionHandler> logger)
    {
        _logger = logger;
    }
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is ForbiddenException)
        {
            _logger.LogError(exception,"Exception occured: {Message}", exception.Message);

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Title = "Forbidden",
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