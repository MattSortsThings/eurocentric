using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Eurocentric.Features.Shared.ErrorHandling;

/// <summary>
///     Converts an <see cref="SqlException" /> thrown on the server into an HTTP response with status code 500 or 503 and
///     a serialized <see cref="ProblemDetails" /> object that describes the error but <i>DOES NOT</i> contain the
///     exception message.
/// </summary>
/// <remarks>
///     This class is adapted from a very helpful
///     <a href="https://timdeschryver.dev/blog/translating-exceptions-into-problem-details-responses">blog post</a> by Tim
///     Deschryver.
/// </remarks>
/// <param name="problemDetailsService">Creates the <see cref="ProblemDetails" /> response.</param>
internal sealed class SqlExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    private const int SqlTimeOutErrorNumber = -2;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken _)
    {
        if (exception is not SqlException sqlException)
        {
            return false;
        }

        string title;
        string detail;
        int statusCode;

        if (sqlException.Number == SqlTimeOutErrorNumber)
        {
            httpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            httpContext.Response.Headers.RetryAfter = "120";
            statusCode = StatusCodes.Status503ServiceUnavailable;
            title = "Database timeout";
            detail =
                "SqlException was thrown while handling the request because the database connection or operation timed out. " +
                "Please retry after c.120 seconds.";
        }
        else
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            statusCode = StatusCodes.Status500InternalServerError;
            title = "SQL exception";
            detail = "SqlException was thrown while handling the request.";
        }

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = new ProblemDetails { Title = title, Detail = detail, Status = statusCode }
        });
    }
}
