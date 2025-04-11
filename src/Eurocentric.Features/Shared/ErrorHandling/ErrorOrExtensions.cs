using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.Shared.ErrorHandling;

/// <summary>
///     Extension methods for the <see cref="ErrorOr{TValue}" /> type.
/// </summary>
public static class ErrorOrExtensions
{
    /// <summary>
    ///     Maps the subject to the discriminated union of a successful HTTP result or an unsuccessful HTTP result containing a
    ///     <see cref="ProblemDetails" /> object.
    /// </summary>
    /// <remarks>
    ///     This method is intended to be used as the terminal method in an asynchronous HTTP request handling method
    ///     chain. The <paramref name="responseMapper" /> argument is ignored if the <paramref name="subject" /> argument is in
    ///     an error state.
    /// </remarks>
    /// <param name="subject">A task containing an <see cref="ErrorOr{TValue}" />.</param>
    /// <param name="responseMapper">
    ///     A function applied to a successful <paramref name="subject" />, which maps its value to a
    ///     successful HTTP result.
    /// </param>
    /// <typeparam name="TResponse">The successful response type.</typeparam>
    /// <typeparam name="TResult">The successful HTTP result type.</typeparam>
    /// <returns>
    ///     A task representing the operation. The result of the task is the discriminated union of a successful HTTP
    ///     result or an unsuccessful HTTP result.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="responseMapper" /> is <see langword="null" />.</exception>
    public static async Task<Results<TResult, ProblemHttpResult>> ToResultOrProblemAsync<TResponse, TResult>(
        this Task<ErrorOr<TResponse>> subject,
        Func<TResponse, TResult> responseMapper)
        where TResult : IResult
    {
        ArgumentNullException.ThrowIfNull(responseMapper, nameof(responseMapper));

        ErrorOr<TResponse> errorsOrResponse = await subject.ConfigureAwait(false);

        return errorsOrResponse.IsError
            ? MapToProblemHttpResult(errorsOrResponse.FirstError)
            : responseMapper(errorsOrResponse.Value);
    }

    private static ProblemHttpResult MapToProblemHttpResult(Error error) => TypedResults.Problem(title: error.Code,
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
