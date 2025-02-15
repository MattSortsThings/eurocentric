using ErrorOr;
using MediatR;

namespace Eurocentric.Shared.AppPipeline;

public abstract record Query<TResult> : IRequest<ErrorOr<TResult>>;
