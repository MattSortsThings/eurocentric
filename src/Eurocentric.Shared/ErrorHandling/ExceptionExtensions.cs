using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Shared.ErrorHandling;

internal static class ExceptionExtensions
{
    internal static ProblemDetails MapToProblemDetails(this Exception exception) => new()
    {
        Title = "InternalServerError",
        Status = StatusCodes.Status500InternalServerError,
        Detail = $"{exception.GetType().Name} was thrown while handling the request."
    };
}
