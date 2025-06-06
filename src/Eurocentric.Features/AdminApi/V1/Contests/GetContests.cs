using ErrorOr;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.DomainMapping;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.EFCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V1.Contests;

public sealed record GetContestsResponse(Contest[] Contests);

internal static class GetContests
{
    internal static IEndpointRouteBuilder MapGetContests(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapGet("contests", HandleAsync)
            .WithName(EndpointIds.Contests.GetContests)
            .WithSummary("Get all contests")
            .WithDescription("Retrieves a list of all existing contests, ordered by contest year.")
            .HasApiVersion(1, 0)
            .Produces<GetContestsResponse>()
            .WithTags(EndpointTags.Contests);

        return apiGroup;
    }

    private static async Task<Results<ProblemHttpResult, Ok<GetContestsResponse>>> HandleAsync(IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeQuery()
        .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(TypedResults.Ok);

    private static ErrorOr<Query> InitializeQuery() => new Query();

    internal sealed record Query : IQuery<GetContestsResponse>;

    internal sealed class Handler(AppDbContext dbContext) : IQueryHandler<Query, GetContestsResponse>
    {
        public async Task<ErrorOr<GetContestsResponse>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            Contest[] contests = await dbContext.Contests
                .AsNoTracking()
                .AsSplitQuery()
                .OrderBy(contest => contest.ContestYear)
                .Select(contest => contest.ToContestDto())
                .ToArrayAsync(cancellationToken);

            return ErrorOrFactory.From(new GetContestsResponse(contests));
        }
    }
}
