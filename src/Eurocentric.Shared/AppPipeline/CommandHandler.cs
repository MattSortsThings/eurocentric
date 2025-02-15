using ErrorOr;
using MediatR;

namespace Eurocentric.Shared.AppPipeline;

public abstract class CommandHandler<TCommand, TResult> : IRequestHandler<TCommand, ErrorOr<TResult>>
    where TCommand : IRequest<ErrorOr<TResult>>
{
    public abstract Task<ErrorOr<TResult>> Handle(TCommand command, CancellationToken cancellationToken);
}
