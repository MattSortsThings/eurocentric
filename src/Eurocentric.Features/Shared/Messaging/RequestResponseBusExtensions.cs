using ErrorOr;
using Eurocentric.Features.Shared.ErrorHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using SlimMessageBus;

namespace Eurocentric.Features.Shared.Messaging;

/// <summary>
///     Extension methods for the <see cref="IRequestResponseBus" /> interface.
/// </summary>
internal static class RequestResponseBusExtensions
{
    /// <summary>
    ///     Dispatches the query to the bus and returns the response mapped to <i>EITHER</i> a <see cref="ProblemHttpResult" />
    ///     (when unsuccessful) <i>OR</i> a successful HTTP result.
    /// </summary>
    /// <param name="bus">The request response bus.</param>
    /// <param name="query">The application query.</param>
    /// <param name="responseMapper">Maps a successful response value to an HTTP result.</param>
    /// <param name="cancellationToken">Cancels the asynchronous operation.</param>
    /// <typeparam name="TResponse">The successful response type.</typeparam>
    /// <returns>A new instance of a type that implements <see cref="IResult" />.</returns>
    internal static async Task<IResult> SendWithResponseMapperAsync<TResponse>(this IRequestResponseBus bus,
        IQuery<TResponse> query,
        Func<TResponse, IResult> responseMapper,
        CancellationToken cancellationToken = default)
    {
        ErrorOr<TResponse> errorsOrResponse = await bus.Send(query, cancellationToken: cancellationToken).ConfigureAwait(false);

        return errorsOrResponse.MatchFirst(responseMapper, error => error.ToProblemHttpResult());
    }

    /// <summary>
    ///     Dispatches the command to the bus and returns the response mapped to <i>EITHER</i> a
    ///     <see cref="ProblemHttpResult" /> (when unsuccessful) <i>OR</i> a successful HTTP result.
    /// </summary>
    /// <param name="bus">The request response bus.</param>
    /// <param name="command">The application command.</param>
    /// <param name="responseMapper">Maps a successful response value to an HTTP result.</param>
    /// <param name="cancellationToken">Cancels the asynchronous operation.</param>
    /// <typeparam name="TResponse">The successful response type.</typeparam>
    /// <returns>A new instance of a type that implements <see cref="IResult" />.</returns>
    internal static async Task<IResult> SendWithResponseMapperAsync<TResponse>(this IRequestResponseBus bus,
        ICommand<TResponse> command,
        Func<TResponse, IResult> responseMapper,
        CancellationToken cancellationToken = default)
    {
        ErrorOr<TResponse> errorsOrResponse =
            await bus.Send(command, cancellationToken: cancellationToken).ConfigureAwait(false);

        return errorsOrResponse.MatchFirst(responseMapper, error => error.ToProblemHttpResult());
    }
}
