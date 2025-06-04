using ErrorOr;
using Eurocentric.Features.AdminApi.V0.Common.Constants;
using Eurocentric.Features.AdminApi.V0.Common.Dtos;
using Eurocentric.Features.AdminApi.V0.Common.Mapping;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.InMemoryRepositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V0.Contests;

public sealed record GetContestResponse(Contest Contest);

internal static class GetContest
{
    internal static IEndpointRouteBuilder MapGetContest(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapGet("contests/{contestId:guid}", HandleAsync)
            .WithName(EndpointIds.Contests.GetContest)
            .WithSummary("Get a contest")
            .WithDescription("Retrieves a single contest.")
            .HasApiVersion(0, 1)
            .HasApiVersion(0, 2)
            .Produces<GetContestResponse>()
            .WithTags(EndpointTags.Contests);

        return apiGroup;
    }

    private static async Task<IResult> HandleAsync([FromRoute(Name = "contestId")] Guid contestId,
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeQuery(contestId)
        .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(TypedResults.Ok);

    private static ErrorOr<Query> InitializeQuery(Guid contestId) => ErrorOrFactory.From(new Query(contestId));

    internal sealed record Query(Guid ContestId) : IQuery<GetContestResponse>;

    internal sealed class Handler(InMemoryContestRepository repository) : IQueryHandler<Query, GetContestResponse>
    {
        public async Task<ErrorOr<GetContestResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            Contest? contest = repository.Contests
                .Where(contest => contest.Id == query.ContestId)
                .Select(contest => contest.ToContestDto())
                .FirstOrDefault();

            return contest is not null
                ? ErrorOrFactory.From(new GetContestResponse(contest))
                : Error.NotFound("Contest not found",
                    "No contest exists with the provided contest ID.",
                    new Dictionary<string, object> { ["contestId"] = query.ContestId });
        }
    }
}
