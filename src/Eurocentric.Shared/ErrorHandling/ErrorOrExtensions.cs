using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Shared.ErrorHandling;

/// <summary>
///     Extension methods for the <see cref="ErrorOr{TValue}" /> generic type.
/// </summary>
public static class ErrorOrExtensions
{
    /// <summary>
    ///     Maps this value or error to a successful HTTP result or an unsuccessful result with a <see cref="ProblemDetails" />
    ///     object.
    /// </summary>
    /// <param name="errorOrValue">
    ///     The discriminated union of an instance of type <typeparamref name="TValue" /> or an <see cref="Error" />.
    /// </param>
    /// <param name="happyPathMapper">
    ///     Maps the value to an instance of type <typeparamref name="TResult" /> when <paramref name="errorOrValue" /> is not
    ///     an error. This function is not invoked when <paramref name="errorOrValue" /> is an error.
    /// </param>
    /// <typeparam name="TValue">The successful value type.</typeparam>
    /// <typeparam name="TResult">The successful HTTP response type.</typeparam>
    /// <returns>
    ///     The discriminated union of an instance of type <typeparamref name="TResult" /> or a
    ///     <see cref="ProblemHttpResult" /> instance.
    /// </returns>
    public static Results<TResult, ProblemHttpResult> ToHttpResult<TValue, TResult>(this ErrorOr<TValue> errorOrValue,
        Func<TValue, TResult> happyPathMapper)
        where TResult : IResult =>
        errorOrValue.IsError
            ? TypedResults.Problem(errorOrValue.FirstError.ToProblemDetails())
            : happyPathMapper.Invoke(errorOrValue.Value);

    private static ProblemDetails ToProblemDetails(this Error error) => new()
    {
        Title = error.Code, Detail = error.Description, Status = error.GetStatusCode(), Extensions = error.GetExtensions()
    };

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
