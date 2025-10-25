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

internal static class GetContests
{
    private static async Task<IResult> ExecuteAsync(
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    ) => await bus.DispatchAsync(new Query(), MapToOk, ct);

    private static Ok<GetContestsResponse> MapToOk(int[] contestYears)
    {
        ContestDto[] contestDtos = contestYears
            .Select(year => ContestDto.CreateExample() with { Id = Guid.NewGuid(), ContestYear = year })
            .OrderBy(contest => contest.ContestYear)
            .ToArray();

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

    internal sealed record Query : IQuery<int[]>;

    [UsedImplicitly]
    internal sealed class QueryHandler : IQueryHandler<Query, int[]>
    {
        public async Task<Result<int[], IDomainError>> OnHandle(Query _, CancellationToken ct)
        {
            await Task.CompletedTask;

            return new[] { 2016, 2020, 2025 };
        }
    }
}
