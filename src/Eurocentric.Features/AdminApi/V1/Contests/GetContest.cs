using ErrorOr;
using Eurocentric.Domain.Contests;
using Eurocentric.Domain.Identifiers;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.DomainMapping;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.EFCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;
using ContestDto = Eurocentric.Features.AdminApi.V1.Common.Dtos.Contest;

namespace Eurocentric.Features.AdminApi.V1.Contests;

public sealed record GetContestResponse(ContestDto Contest);

internal static class GetContest
{
    internal static IEndpointRouteBuilder MapGetContest(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapGet("contests/{contestId:guid}", HandleAsync)
            .WithName(EndpointIds.Contests.GetContest)
            .WithSummary("Get a contest")
            .WithDescription("Retrieves a single contest.")
            .HasApiVersion(1, 0)
            .Produces<GetContestResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithTags(EndpointTags.Contests);

        return apiGroup;
    }

    private static async Task<Results<ProblemHttpResult, Ok<GetContestResponse>>> HandleAsync(
        [FromRoute(Name = "contestId")] Guid contestId,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeQuery(contestId)
        .ThenAsync(contest => bus.Send(contest, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(TypedResults.Ok);

    private static ErrorOr<Query> InitializeQuery(Guid contestId) => ErrorOrFactory.From(new Query(contestId));

    internal sealed record Query(Guid ContestId) : IQuery<GetContestResponse>;

    internal sealed class Handler(AppDbContext dbContext) : IQueryHandler<Query, GetContestResponse>
    {
        public async Task<ErrorOr<GetContestResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            ContestId contestId = ContestId.FromValue(query.ContestId);

            ContestDto? contest = await dbContext.Contests
                .AsNoTracking()
                .AsSplitQuery()
                .Where(contest => contest.Id == contestId)
                .Select(contest => contest.ToContestDto())
                .FirstOrDefaultAsync(cancellationToken);

            return contest is not null
                ? new GetContestResponse(contest)
                : ContestErrors.ContestNotFound(contestId);
        }
    }
}
