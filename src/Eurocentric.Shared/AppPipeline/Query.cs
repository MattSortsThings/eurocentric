using ErrorOr;
using MediatR;

namespace Eurocentric.Shared.AppPipeline;

/// <summary>
///     Abstract base class for a generic query that returns the discriminated union of a result or a list of errors.
/// </summary>
/// <typeparam name="TResult">The happy path result type.</typeparam>
public abstract record Query<TResult> : IRequest<ErrorOr<TResult>>;
