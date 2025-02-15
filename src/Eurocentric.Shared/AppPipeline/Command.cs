using ErrorOr;
using MediatR;

namespace Eurocentric.Shared.AppPipeline;

public abstract record Command<TResult> : IRequest<ErrorOr<TResult>>;
