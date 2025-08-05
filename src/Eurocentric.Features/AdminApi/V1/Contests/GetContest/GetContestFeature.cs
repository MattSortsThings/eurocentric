using ErrorOr;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.Shared.Messaging;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V1.Contests.GetContest;

internal static class GetContestFeature
{
    internal static async Task<IResult> ExecuteAsync([FromRoute(Name = "contestId")] Guid contestId,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) =>
        await bus.SendWithResponseMapperAsync(new Query(contestId), TypedResults.Ok, cancellationToken);

    internal sealed record Query(Guid ContestId) : IQuery<GetContestResponse>;

    [UsedImplicitly]
    internal sealed class QueryHandler : IQueryHandler<Query, GetContestResponse>
    {
        public async Task<ErrorOr<GetContestResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            Contest dummyContest = Contest.CreateExample() with { Id = query.ContestId };

            return new GetContestResponse(dummyContest);
        }
    }
}
