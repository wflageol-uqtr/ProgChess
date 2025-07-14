using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ProChess.Server.Exceptions;

public class ExecutionErrorExceptionHandler: IExceptionHandler
{
    private readonly ILogger<ExecutionErrorExceptionHandler> _logger;

    public ExecutionErrorExceptionHandler(ILogger<ExecutionErrorExceptionHandler> logger)
    {
        _logger = logger;
    }
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is ExecutionErrorException)
        {
            _logger.LogError(exception,"Exception occured: {Message}", exception.Message);

            var problemDetails = new ProblemDetails
            {
                Status = 600,
                Title = "Execution Failed",
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