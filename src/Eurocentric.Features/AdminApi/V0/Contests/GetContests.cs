using ErrorOr;
using Eurocentric.Features.AdminApi.V0.Common.Constants;
using Eurocentric.Features.AdminApi.V0.Common.Contracts;
using Eurocentric.Features.AdminApi.V0.Common.Extensions;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V0.Contests;

public sealed record GetContestsResponse(Contest[] Contests);

internal static class GetContests
{
    internal static IEndpointRouteBuilder MapGetContests(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapGet("contests", ExecuteAsync)
            .WithName(EndpointNames.Contests.GetContests)
            .WithSummary("Get all contests")
            .WithDescription("Retrieves all contests in contest year order.")
            .WithTags(EndpointTags.Contests)
            .HasApiVersion(0, 2)
            .Produces<GetContestsResponse>();

        return apiGroup;
    }

    private static async Task<Results<ProblemHttpResult, Ok<GetContestsResponse>>> ExecuteAsync(
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeQuery()
        .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(TypedResults.Ok);

    private static ErrorOr<Query> InitializeQuery() => ErrorOrFactory.From(new Query());

    internal sealed record Query : IQuery<GetContestsResponse>;

    internal sealed class Handler(AppDbContext dbContext) : IQueryHandler<Query, GetContestsResponse>
    {
        public async Task<ErrorOr<GetContestsResponse>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            Contest[] contests = await dbContext.PlaceholderContests.AsNoTracking()
                .OrderBy(contest => contest.ContestYear)
                .Select(Projections.ProjectToContestDto)
                .ToArrayAsync(cancellationToken);

            return ErrorOrFactory.From(new GetContestsResponse(contests));
        }
    }
}
