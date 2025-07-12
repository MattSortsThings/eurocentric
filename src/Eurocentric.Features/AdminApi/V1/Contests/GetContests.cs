using ErrorOr;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Contracts;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
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
    internal static IEndpointRouteBuilder MapGetContests(this IEndpointRouteBuilder v1Group)
    {
        v1Group.MapGet("contests", ExecuteAsync)
            .WithName(EndpointConstants.Names.Contests.GetContests)
            .WithSummary("Get all countries")
            .WithDescription("Retrieves all existing countries in country code order.")
            .WithTags(EndpointConstants.Tags.Contests)
            .HasApiVersion(1, 0)
            .Produces<GetContestsResponse>();

        return v1Group;
    }

    private static async Task<Results<ProblemHttpResult, Ok<GetContestsResponse>>> ExecuteAsync(IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeQuery()
        .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(TypedResults.Ok);

    private static ErrorOr<Query> InitializeQuery() => ErrorOrFactory.From(new Query());

    internal sealed record Query : IQuery<GetContestsResponse>;

    internal sealed class Handler(AppDbContext dbContext) : IQueryHandler<Query, GetContestsResponse>
    {
        public async Task<ErrorOr<GetContestsResponse>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            Contest[] contests = await dbContext.Contests.AsNoTracking()
                .AsSplitQuery()
                .OrderBy(contest => contest.ContestYear)
                .Select(contest => contest.ToContestDto())
                .ToArrayAsync(cancellationToken);

            return new GetContestsResponse(contests);
        }
    }
}
