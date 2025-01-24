using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Shared.ErrorHandling;

internal static class BadHttpRequestExceptionExtensions
{
    internal static ProblemDetails MapToProblemDetails(this BadHttpRequestException exception)
    {
        Dictionary<string, object?> extensions = exception.InnerException is JsonException j
            ? new Dictionary<string, object?>
            {
                { "error", exception.Message.Replace("\"", "'") },
                { "jsonError", j.Message.Replace("\"", "'") },
                { "jsonPath", j.Path }
            }
            : new Dictionary<string, object?> { { "error", exception.Message.Replace("\"", "'") } };

        return new ProblemDetails
        {
            Title = "BadHttpRequest",
            Status = StatusCodes.Status400BadRequest,
            Detail = $"{exception.GetType().Name} was thrown while handling the request.",
            Extensions = extensions
        };
    }
}
