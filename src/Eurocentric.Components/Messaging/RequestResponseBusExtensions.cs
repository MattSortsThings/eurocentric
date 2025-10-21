using CSharpFunctionalExtensions;
using Eurocentric.Components.ErrorHandling;
using Eurocentric.Domain.Functional;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SlimMessageBus;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Eurocentric.Components.Messaging;

/// <summary>
///     Extension methods for the <see cref="IRequestResponseBus" /> interface.
/// </summary>
public static class RequestResponseBusExtensions
{
    /// <summary>
    ///     Asynchronously dispatches the provided query to the messaging bus and returns <i>either</i> the successful
    ///     result value mapped to an HTTP result <i>or</i> the error mapped to an HTTP result containing a
    ///     <see cref="ProblemDetails" /> object.
    /// </summary>
    /// <param name="bus">The request-response bus.</param>
    /// <param name="query">The query to be dispatched.</param>
    /// <param name="valueMapper">The function to be invoked on the return value if the query succeeds.</param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to be observed while waiting for the asynchronous dispatch operation to
    ///     complete.
    /// </param>
    /// <typeparam name="TValue">The successful return value type for the query.</typeparam>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous dispatch operation. The task result contains the mapped
    ///     HTTP result.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="query" /> is <see langword="null" />; or,
    ///     <paramref name="valueMapper" /> is <see langword="null" />.
    /// </exception>
    public static async Task<IResult> DispatchAsync<TValue>(
        this IRequestResponseBus bus,
        IQuery<TValue> query,
        Func<TValue, IResult> valueMapper,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(valueMapper);

        Result<TValue, IDomainError> result = await bus.Send(query, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return result.IsSuccess ? valueMapper(result.Value) : TypedResults.Problem(result.Error.ToProblemDetails());
    }

    /// <summary>
    ///     Asynchronously dispatches the provided command to the messaging bus and returns <i>either</i> the successful
    ///     result value mapped to an HTTP result <i>or</i> the error mapped to an HTTP result containing a
    ///     <see cref="ProblemDetails" /> object.
    /// </summary>
    /// <param name="bus">The request-response bus.</param>
    /// <param name="command">The command to be dispatched.</param>
    /// <param name="valueMapper">The function to be invoked on the return value if the command succeeds.</param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to be observed while waiting for the asynchronous dispatch operation to
    ///     complete.
    /// </param>
    /// <typeparam name="TValue">The successful return value type for the command.</typeparam>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous dispatch operation. The task result contains the mapped
    ///     HTTP result.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="command" /> is <see langword="null" />; or,
    ///     <paramref name="valueMapper" /> is <see langword="null" />.
    /// </exception>
    public static async Task<IResult> DispatchAsync<TValue>(
        this IRequestResponseBus bus,
        ICommand<TValue> command,
        Func<TValue, IResult> valueMapper,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentNullException.ThrowIfNull(valueMapper);

        Result<TValue, IDomainError> result = await bus.Send(command, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return result.IsSuccess ? valueMapper(result.Value) : TypedResults.Problem(result.Error.ToProblemDetails());
    }

    /// <summary>
    ///     Asynchronously dispatches the provided unit command to the messaging bus and returns <i>either</i> an HTTP
    ///     result with status code 204 (No Content) when the unit command succeeds <i>or</i> the error mapped to an HTTP
    ///     result containing a <see cref="ProblemDetails" /> object.
    /// </summary>
    /// <param name="bus">The request-response bus.</param>
    /// <param name="unitCommand">The unit command to be dispatched.</param>
    /// <param name="cancellationToken">
    ///     A <see cref="CancellationToken" /> to be observed while waiting for the asynchronous dispatch operation to
    ///     complete.
    /// </param>
    /// <returns>
    ///     A <see cref="Task" /> representing the asynchronous dispatch operation. The task result contains the mapped
    ///     HTTP result.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="unitCommand" /> is <see langword="null" />.</exception>
    public static async Task<IResult> DispatchAsync(
        this IRequestResponseBus bus,
        IUnitCommand unitCommand,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(unitCommand);

        UnitResult<IDomainError> result = await bus.Send(unitCommand, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return result.IsSuccess ? TypedResults.NoContent() : TypedResults.Problem(result.Error.ToProblemDetails());
    }
}
