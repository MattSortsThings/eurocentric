using ErrorOr;
using MediatR;

namespace Eurocentric.Shared.AppPipeline;

public abstract class QueryHandler<TQuery, TResult> : IRequestHandler<TQuery, ErrorOr<TResult>>
    where TQuery : Query<TResult>
{
    public abstract Task<ErrorOr<TResult>> Handle(TQuery query, CancellationToken cancellationToken);
}
