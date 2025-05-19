using ErrorOr;
using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.EfCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;
using Contest = Eurocentric.Features.AdminApi.V1.Common.Dtos.Contest;

namespace Eurocentric.Features.AdminApi.V1.Contests;

public sealed record GetContestResponse(Contest Contest);

internal static class GetContest
{
    internal static IEndpointRouteBuilder MapGetContest(this IEndpointRouteBuilder apiVersionGroup)
    {
        apiVersionGroup.MapGet("contests/{contestId:guid}", Endpoint.HandleAsync)
            .WithName(RouteIds.Contests.GetContest)
            .HasApiVersion(1, 0)
            .WithSummary("Get a contest")
            .WithDescription("Retrieves a single contest.")
            .WithTags(EndpointTags.Contests)
            .Produces<GetContestResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        return apiVersionGroup;
    }

    internal sealed record Query(Guid ContestId) : IQuery<GetContestResponse>;

    internal sealed class Handler(AppDbContext dbContext) : IQueryHandler<Query, GetContestResponse>
    {
        public async Task<ErrorOr<GetContestResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            ContestId contestId = ContestId.FromValue(query.ContestId);

            Contest? contest = await dbContext.Contests.AsNoTracking()
                .Where(c => c.Id == contestId)
                .Select(c => c.ToContestDto())
                .FirstOrDefaultAsync(cancellationToken);

            return contest is null ? ContestErrors.ContestNotFound(contestId) : new GetContestResponse(contest);
        }
    }

    private static class Endpoint
    {
        internal static async Task<Results<Ok<GetContestResponse>, ProblemHttpResult>> HandleAsync(
            [FromRoute(Name = "contestId")] Guid contestId,
            IRequestResponseBus bus,
            CancellationToken cancellationToken = default) => await InitializeQuery(contestId)
            .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
            .ToResultOrProblemAsync(TypedResults.Ok);

        private static ErrorOr<Query> InitializeQuery(Guid contestId) => ErrorOrFactory.From(new Query(contestId));
    }
}
