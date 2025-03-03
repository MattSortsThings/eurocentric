using ErrorOr;
using MediatR;

namespace Eurocentric.Shared.AppPipeline;

/// <summary>
///     Abstract base class for a handler that receives an application query and returns the discriminated union of
///     <i>either</i> a result <i>or</i> a list of errors.
/// </summary>
/// <typeparam name="TQuery">The query type.</typeparam>
/// <typeparam name="TResult">The happy path result type.</typeparam>
public abstract class QueryHandler<TQuery, TResult> : IRequestHandler<TQuery, ErrorOr<TResult>>
    where TQuery : Query<TResult>
{
    public abstract Task<ErrorOr<TResult>> Handle(TQuery query, CancellationToken cancellationToken);
}
