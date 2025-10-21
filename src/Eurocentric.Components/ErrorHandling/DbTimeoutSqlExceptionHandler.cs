using Eurocentric.Components.DataAccess.Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Eurocentric.Components.ErrorHandling;

/// <summary>
///     Converts an <see cref="DbTimeoutSqlExceptionHandler" /> thrown on the server into an HTTP response with a
///     serialized <see cref="ProblemDetails" /> object that does not contain the exception message.
/// </summary>
/// <remarks>
///     This class is adapted from a very helpful
///     <a href="https://timdeschryver.dev/blog/translating-exceptions-into-problem-details-responses">blog post</a> by Tim
///     Deschryver.
/// </remarks>
/// <param name="problemDetailsService">Creates the <see cref="ProblemDetails" /> response.</param>
/// <param name="options">Contains options used for connecting to the Azure SQL database.</param>
internal sealed class DbTimeoutSqlExceptionHandler(
    IProblemDetailsService problemDetailsService,
    IOptions<AzureSqlDbOptions> options
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken _)
    {
        if (exception is not SqlException sqlException || !sqlException.CausedByDbTimeout())
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
        httpContext.Response.Headers.RetryAfter = options.Value.HttpRetryAfterSeconds.ToString();

        return await problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = new ProblemDetails
                {
                    Title = "Database timeout",
                    Detail =
                        "SqlException due to database timeout was thrown while handling the request. "
                        + "Please retry after the interval specified in the \"Retry-After\" header.",
                    Status = StatusCodes.Status503ServiceUnavailable,
                },
            }
        );
    }
}
