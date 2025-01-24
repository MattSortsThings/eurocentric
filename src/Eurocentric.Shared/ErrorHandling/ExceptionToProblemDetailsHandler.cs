using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Shared.ErrorHandling;

/// <summary>
///     Maps an exception to a problem details response.
/// </summary>
/// <remarks>
///     This class is adapted from a very helpful
///     <a href="https://timdeschryver.dev/blog/translating-exceptions-into-problem-details-responses">blog post</a>
///     by Tim Deschryver.
/// </remarks>
/// <param name="problemDetailsService">Creates the <see cref="ProblemDetails" /> response.</param>
internal sealed class ExceptionToProblemDetailsHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken = default)
    {
        ProblemDetails problemDetails;

        if (exception is BadHttpRequestException b)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            problemDetails = b.MapToProblemDetails();
        }
        else
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            problemDetails = exception.MapToProblemDetails();
        }

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext, ProblemDetails = problemDetails
        });
    }
}
