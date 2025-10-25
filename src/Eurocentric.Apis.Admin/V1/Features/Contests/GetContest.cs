using CSharpFunctionalExtensions;
using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Messaging;
using Eurocentric.Domain.Core;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using ContestDto = Eurocentric.Apis.Admin.V1.Dtos.Contests.Contest;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Eurocentric.Apis.Admin.V1.Features.Contests;

internal static class GetContest
{
    private static async Task<IResult> ExecuteAsync(
        [FromRoute(Name = "contestId")] Guid contestId,
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    ) => await bus.DispatchAsync(contestId.ToQuery(), MapToOk, ct);

    private static Query ToQuery(this Guid contestId) => new(contestId);

    private static Ok<GetContestResponse> MapToOk(Guid contestId)
    {
        ContestDto contestDto = ContestDto.CreateExample() with { Id = contestId };

        return TypedResults.Ok(new GetContestResponse(contestDto));
    }

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapGet("contests/{contestId:guid}", ExecuteAsync)
                .WithName(V1Endpoints.Contests.GetContest)
                .AddedInVersion1Point0()
                .WithSummary("Get aa contest")
                .WithDescription("Retrieves the requested contest.")
                .WithTags(V1Tags.Contests)
                .Produces<GetContestResponse>()
                .ProducesProblem(StatusCodes.Status404NotFound);
        }
    }

    internal sealed record Query(Guid ContestId) : IQuery<Guid>;

    [UsedImplicitly]
    internal sealed class QueryHandler : IQueryHandler<Query, Guid>
    {
        public async Task<Result<Guid, IDomainError>> OnHandle(Query query, CancellationToken ct)
        {
            await Task.CompletedTask;

            return query.ContestId;
        }
    }
}
