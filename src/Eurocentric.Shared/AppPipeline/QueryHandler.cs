using ErrorOr;
using MediatR;

namespace Eurocentric.Shared.AppPipeline;

/// <summary>
///     Abstract base class for a generic query handler that returns the discriminated union of a result or a list of
///     errors.
/// </summary>
/// <typeparam name="TQuery">The query type.</typeparam>
/// <typeparam name="TResult">The happy path result type.</typeparam>
public abstract class QueryHandler<TQuery, TResult> : IRequestHandler<TQuery, ErrorOr<TResult>>
    where TQuery : Query<TResult>
{
    public abstract Task<ErrorOr<TResult>> Handle(TQuery query, CancellationToken cancellationToken);
}
