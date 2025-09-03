using ErrorOr;
using Eurocentric.Features.PublicApi.V0.Common.Enums;
using Eurocentric.Features.Shared.Messaging;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SlimMessageBus;

namespace Eurocentric.Features.PublicApi.V0.Queryables.GetQueryableContestStages;

internal static class GetQueryableContestStagesFeature
{
    internal static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(new Query(), TypedResults.Ok, cancellationToken);

    internal sealed record Query : IQuery<GetQueryableContestStagesResponse>;

    [UsedImplicitly]
    internal sealed class QueryHandler : IQueryHandler<Query, GetQueryableContestStagesResponse>
    {
        public Task<ErrorOr<GetQueryableContestStagesResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            QueryableContestStage[] values = Enum.GetValues<QueryableContestStage>();

            ErrorOr<GetQueryableContestStagesResponse> response = new GetQueryableContestStagesResponse(values);

            return Task.FromResult(response);
        }
    }
}
