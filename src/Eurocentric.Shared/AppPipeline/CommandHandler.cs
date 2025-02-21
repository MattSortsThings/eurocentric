using ErrorOr;
using MediatR;

namespace Eurocentric.Shared.AppPipeline;

/// <summary>
///     Abstract base class for a generic command handler that returns the discriminated union of a result or a list of
///     errors.
/// </summary>
/// <typeparam name="TCommand">The command type.</typeparam>
/// <typeparam name="TResult">The happy path result type.</typeparam>
public abstract class CommandHandler<TCommand, TResult> : IRequestHandler<TCommand, ErrorOr<TResult>>
    where TCommand : Command<TResult>
{
    public abstract Task<ErrorOr<TResult>> Handle(TCommand command, CancellationToken cancellationToken);
}
