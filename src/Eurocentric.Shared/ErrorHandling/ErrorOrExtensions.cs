using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Shared.ErrorHandling;

/// <summary>
///     Extension methods for the <see cref="ErrorOr{TValue}" /> type.
/// </summary>
public static class ErrorOrExtensions
{
    /// <summary>
    ///     Maps the discriminated union of errors or a value to the discriminated union of a problem HTTP result or a
    ///     successful HTTP result.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Given an <paramref name="errorOrValue" /> argument having <see cref="IErrorOr.IsError" /> =
    ///         <see langword="true" />, the method maps the first error in the underlying collection to a
    ///         <see cref="ProblemDetails" /> object and returns it as a <see cref="ProblemHttpResult" /> instance.
    ///     </para>
    ///     <para>
    ///         Given an <paramref name="errorOrValue" /> argument having <see cref="IErrorOr.IsError" /> =
    ///         <see langword="false" />, the method invokes the <paramref name="mapper" /> function argument on its underlying
    ///         value and returns the result, which is of <typeparamref name="TResult" />.
    ///     </para>
    /// </remarks>
    /// <param name="errorOrValue">
    ///     The discriminated union of <i>either</i> a collection of <see cref="Error" /> instances <i>or</i> an instance of
    ///     type <typeparamref name="TValue" />.
    /// </param>
    /// <param name="mapper">
    ///     Maps an instance of type <typeparamref name="TValue" /> to an instance of type <typeparamref name="TResult" />.
    /// </param>
    /// <typeparam name="TValue">The successful value type.</typeparam>
    /// <typeparam name="TResult">The successful HTTP result type.</typeparam>
    /// <returns>
    ///     An <see cref="IResult" /> that is <i>either</i> of type <see cref="ProblemHttpResult" /> <i>or</i> of type
    ///     <typeparamref name="TResult" />.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="mapper" /> is <see langword="null" />.</exception>
    public static Results<TResult, ProblemHttpResult> MapToResults<TValue, TResult>(this ErrorOr<TValue> errorOrValue,
        Func<TValue, TResult> mapper)
        where TResult : IResult
    {
        ArgumentNullException.ThrowIfNull(mapper);

        return errorOrValue.IsError ? errorOrValue.FirstError.MapToProblemHttpResult() : mapper.Invoke(errorOrValue.Value);
    }

    private static ProblemHttpResult MapToProblemHttpResult(this Error error) =>
        TypedResults.Problem(statusCode: error.GetStatusCode(),
            title: error.Code,
            detail: error.Description,
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
        error.Metadata?.ToDictionary(pair => pair.Key, object? (pair) => pair.Value) ?? [];
}
