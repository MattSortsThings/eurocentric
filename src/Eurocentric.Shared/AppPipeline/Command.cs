using ErrorOr;
using MediatR;

namespace Eurocentric.Shared.AppPipeline;

/// <summary>
///     Abstract base class for an application command that returns the discriminated union of <i>either</i> a result
///     <i>or</i> a list of errors.
/// </summary>
/// <typeparam name="TResult">The happy path result type.</typeparam>
public abstract record Command<TResult> : IRequest<ErrorOr<TResult>>;
