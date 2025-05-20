using ErrorOr;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.EfCore;
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
    internal static IEndpointRouteBuilder MapGetContests(this IEndpointRouteBuilder apiVersionGroup)
    {
        apiVersionGroup.MapGet("contests", Endpoint.HandleAsync)
            .WithName(RouteIds.Contests.GetContests)
            .HasApiVersion(1, 0)
            .WithSummary("Get all contests")
            .WithDescription("Retrieves all existing contests, ordered by contest year.")
            .WithTags(EndpointTags.Contests)
            .Produces<GetContestsResponse>();

        return apiVersionGroup;
    }

    internal sealed record Query : IQuery<GetContestsResponse>;

    internal sealed class Handler(AppDbContext dbContext) : IQueryHandler<Query, GetContestsResponse>
    {
        public async Task<ErrorOr<GetContestsResponse>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            Contest[] contests = await dbContext.Contests.AsNoTracking()
                .OrderBy(contest => contest.Year)
                .Select(contest => contest.ToContestDto())
                .ToArrayAsync(cancellationToken);

            return ErrorOrFactory.From(new GetContestsResponse(contests));
        }
    }

    private static class Endpoint
    {
        internal static async Task<Results<Ok<GetContestsResponse>, ProblemHttpResult>> HandleAsync(IRequestResponseBus bus,
            CancellationToken cancellationToken = default) => await InitializeQuery()
            .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
            .ToResultOrProblemAsync(TypedResults.Ok);

        private static ErrorOr<Query> InitializeQuery() => ErrorOrFactory.From(new Query());
    }
}
