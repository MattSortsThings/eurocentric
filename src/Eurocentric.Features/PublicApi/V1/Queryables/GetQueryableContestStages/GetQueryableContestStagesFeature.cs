using ErrorOr;
using Eurocentric.Features.PublicApi.V1.Common.Enums;
using Eurocentric.Features.Shared.Messaging;
using Microsoft.AspNetCore.Http;
using SlimMessageBus;

namespace Eurocentric.Features.PublicApi.V1.Queryables.GetQueryableContestStages;

internal static class GetQueryableContestStagesFeature
{
    internal static async Task<IResult> ExecuteAsync(IRequestResponseBus bus, CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(new Query(), TypedResults.Ok, cancellationToken);

    internal sealed record Query : IQuery<GetQueryableContestStagesResponse>;

    internal sealed class QueryHandler : IQueryHandler<Query, GetQueryableContestStagesResponse>
    {
        public Task<ErrorOr<GetQueryableContestStagesResponse>> OnHandle(Query query,
            CancellationToken cancellationToken) =>
            Task.FromResult(new GetQueryableContestStagesResponse(Enum.GetValues<QueryableContestStage>()).ToErrorOr());
    }
}
