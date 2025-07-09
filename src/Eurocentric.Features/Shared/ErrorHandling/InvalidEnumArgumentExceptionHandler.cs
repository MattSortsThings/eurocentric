using System.ComponentModel;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.Shared.ErrorHandling;

/// <summary>
///     Converts an <see cref="InvalidEnumArgumentException" /> thrown on the server into an HTTP response with status code
///     400 and a serialized <see cref="ProblemDetails" /> object that includes the exception message.
/// </summary>
/// <remarks>
///     This class is adapted from a very helpful
///     <a href="https://timdeschryver.dev/blog/translating-exceptions-into-problem-details-responses">blog post</a> by Tim
///     Deschryver.
/// </remarks>
/// <param name="problemDetailsService">Creates the <see cref="ProblemDetails" /> response.</param>
internal sealed class InvalidEnumArgumentExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken _)
    {
        if (exception is not InvalidEnumArgumentException httpException)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = new ProblemDetails
            {
                Title = "Invalid enum argument",
                Detail = "InvalidEnumArgumentException was thrown while handling the request.",
                Status = StatusCodes.Status400BadRequest,
                Extensions = { ["exceptionMessage"] = httpException.Message }
            }
        });
    }
}
