using ErrorOr;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Messaging;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using SlimMessageBus;

namespace Eurocentric.Features.PublicApi.V1.Queryables.GetQueryableVotingMethods;

internal static class GetQueryableVotingMethodsFeature
{
    internal static async Task<IResult> ExecuteAsync(IRequestResponseBus bus, CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(new Query(), TypedResults.Ok, cancellationToken);

    internal sealed record Query : IQuery<GetQueryableVotingMethodsResponse>;

    [UsedImplicitly]
    internal sealed class QueryHandler : IQueryHandler<Query, GetQueryableVotingMethodsResponse>
    {
        public Task<ErrorOr<GetQueryableVotingMethodsResponse>> OnHandle(Query query,
            CancellationToken cancellationToken) =>
            Task.FromResult(new GetQueryableVotingMethodsResponse(Enum.GetValues<QueryableVotingMethod>()).ToErrorOr());
    }
}
