using ErrorOr;
using Eurocentric.Features.PublicApi.V0.Common.Enums;
using Eurocentric.Features.Shared.Messaging;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SlimMessageBus;

namespace Eurocentric.Features.PublicApi.V0.Queryables.GetQueryableVotingMethods;

internal static class GetQueryableVotingMethodsFeature
{
    internal static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default)
    {
        ErrorOr<GetQueryableVotingMethodsResponse> errorsOrResponse =
            await bus.Send(new Query(), cancellationToken: cancellationToken);

        return TypedResults.Ok(errorsOrResponse.Value);
    }

    internal sealed record Query : IQuery<GetQueryableVotingMethodsResponse>;

    [UsedImplicitly]
    internal sealed class QueryHandler : IQueryHandler<Query, GetQueryableVotingMethodsResponse>
    {
        public Task<ErrorOr<GetQueryableVotingMethodsResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            QueryableVotingMethod[] values = Enum.GetValues<QueryableVotingMethod>();

            ErrorOr<GetQueryableVotingMethodsResponse> response = new GetQueryableVotingMethodsResponse(values);

            return Task.FromResult(response);
        }
    }
}
