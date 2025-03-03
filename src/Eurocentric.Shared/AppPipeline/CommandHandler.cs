using ErrorOr;
using MediatR;

namespace Eurocentric.Shared.AppPipeline;

/// <summary>
///     Abstract base class for a handler that receives an application command and returns the discriminated union of
///     <i>either</i> a result <i>or</i> a list of errors.
/// </summary>
/// <typeparam name="TCommand">The command type.</typeparam>
/// <typeparam name="TResult">The happy path result type.</typeparam>
public abstract class CommandHandler<TCommand, TResult> : IRequestHandler<TCommand, ErrorOr<TResult>>
    where TCommand : Command<TResult>
{
    public abstract Task<ErrorOr<TResult>> Handle(TCommand command, CancellationToken cancellationToken);
}
