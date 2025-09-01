using ErrorOr;
using SlimMessageBus;

namespace Eurocentric.Features.Shared.Messaging;

/// <summary>
///     An application query handler that <i>EITHER</i> fails and returns a list of <see cref="Error" /> objects <i>OR</i>
///     succeeds and returns a response of type <typeparamref name="TResponse" />.
/// </summary>
/// <typeparam name="TQuery">The query type.</typeparam>
/// <typeparam name="TResponse">The successful response type.</typeparam>
internal interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, ErrorOr<TResponse>>
    where TQuery : IQuery<TResponse>;
