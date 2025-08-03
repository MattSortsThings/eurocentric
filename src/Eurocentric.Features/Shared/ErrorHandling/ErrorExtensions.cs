using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.Shared.ErrorHandling;

/// <summary>
///     Extension methods for the <see cref="Error" /> type.
/// </summary>
internal static class ErrorExtensions
{
    /// <summary>
    ///     Maps an <see cref="Error" /> to an unsuccessful HTTP result with a <see cref="ProblemDetails" /> object.
    /// </summary>
    /// <param name="error">The error to be mapped.</param>
    /// <returns>A new <see cref="ProblemHttpResult" /> instance.</returns>
    internal static ProblemHttpResult ToProblemHttpResult(this Error error) => TypedResults.Problem(title: error.Code,
        detail: error.Description,
        statusCode: error.GetStatusCode(),
        extensions: error.GetExtensions());

    private static int GetStatusCode(this Error error) => error.Type switch
    {
        ErrorType.Failure => StatusCodes.Status422UnprocessableEntity,
        ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
        ErrorType.Forbidden => StatusCodes.Status403Forbidden,
        _ => StatusCodes.Status500InternalServerError
    };

    private static Dictionary<string, object?> GetExtensions(this Error error) =>
        error.Metadata?.ToDictionary(kvp => kvp.Key, object? (kvp) => kvp.Value)
        ?? [];
}
