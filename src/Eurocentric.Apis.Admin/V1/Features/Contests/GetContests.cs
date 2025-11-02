using CSharpFunctionalExtensions;
using Eurocentric.Apis.Admin.V1.Config;
using Eurocentric.Apis.Admin.V1.Dtos.Contests;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Components.Messaging;
using Eurocentric.Domain.Aggregates.Contests;
using Eurocentric.Domain.Core;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using ContestAggregate = Eurocentric.Domain.Aggregates.Contests.Contest;
using ContestDto = Eurocentric.Apis.Admin.V1.Dtos.Contests.Contest;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Eurocentric.Apis.Admin.V1.Features.Contests;

internal static class GetContests
{
    private static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    ) => await bus.DispatchAsync(new Query(), MapToOk, ct);

    private static Ok<GetContestsResponse> MapToOk(ContestAggregate[] aggregates)
    {
        ContestDto[] contestDtos = aggregates.Select(contest => contest.ToDto()).ToArray();

        return TypedResults.Ok(new GetContestsResponse(contestDtos));
    }

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapGet("contests", ExecuteAsync)
                .WithName(V1Endpoints.Contests.GetContests)
                .AddedInVersion1Point0()
                .WithSummary("Get all contests")
                .WithDescription("Retrieves a list of all existing contests, ordered by contest year.")
                .WithTags(V1Tags.Contests)
                .Produces<GetContestsResponse>();
        }
    }

    internal sealed record Query : IQuery<ContestAggregate[]>;

    [UsedImplicitly]
    internal sealed class QueryHandler(IContestReadRepository readRepository) : IQueryHandler<Query, ContestAggregate[]>
    {
        public async Task<Result<ContestAggregate[], IDomainError>> OnHandle(Query _, CancellationToken ct) =>
            await readRepository.GetAllUntrackedAsync(contest => contest.ContestYear, ct);
    }
}
