using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.Shared.ErrorHandling;

/// <summary>
///     Extensions methods for the <see cref="ErrorOr{T}" /> class.
/// </summary>
internal static class ErrorOrExtensions
{
    /// <summary>
    ///     Maps the subject to the discriminated union of <i>EITHER</i> an unsuccessful HTTP result with a
    ///     <seealso cref="ProblemDetails" /> response object <i>OR</i> a successful HTTP result with an optional response
    ///     object.
    /// </summary>
    /// <remarks>
    ///     This method is intended to be used as the terminal method in an asynchronous HTTP request handling method
    ///     chain. The <paramref name="responseMapper" /> argument is ignored if the <paramref name="errorsOrResponse" />
    ///     argument is in an error state.
    /// </remarks>
    /// <param name="errorsOrResponse">
    ///     A task representing an asynchronous operation that returns either errors or a value. The task's result contains the
    ///     result of the operation.
    /// </param>
    /// <param name="responseMapper">
    ///     A function applied to a successful <paramref name="errorsOrResponse" /> argument, which maps its value to a
    ///     successful HTTP result.
    /// </param>
    /// <typeparam name="TResponse">The successful response type.</typeparam>
    /// <typeparam name="TResult">The successful HTTP result type.</typeparam>
    /// <returns>
    ///     A task representing the operation. The result of the task is an <see cref="IResult" /> that is either an
    ///     unsuccessful HTTP result or a successful HTTP result.
    /// </returns>
    internal static async Task<Results<ProblemHttpResult, TResult>> ToProblemOrResponseAsync<TResponse, TResult>(
        this Task<ErrorOr<TResponse>> errorsOrResponse,
        Func<TResponse, TResult> responseMapper)
        where TResult : IResult
    {
        ArgumentNullException.ThrowIfNull(responseMapper);

        ErrorOr<TResponse> result = await errorsOrResponse.ConfigureAwait(false);

        return result.IsError ? result.FirstError.ToProblemHttpResult() : responseMapper(result.Value);
    }
}
