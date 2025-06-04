using ErrorOr;
using Eurocentric.Features.AdminApi.V0.Common.Constants;
using Eurocentric.Features.AdminApi.V0.Common.Dtos;
using Eurocentric.Features.AdminApi.V0.Common.Mapping;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.InMemoryRepositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V0.Contests;

public sealed record GetContestsResponse(Contest[] Contests);

internal static class GetContests
{
    internal static IEndpointRouteBuilder MapGetContests(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapGet("contests", HandleAsync)
            .WithName(EndpointIds.Contests.GetContests)
            .WithSummary("Get all contests")
            .WithDescription("Retrieves a list of all existing contests, ordered by contest year.")
            .HasApiVersion(0, 1)
            .HasApiVersion(0, 2)
            .Produces<GetContestsResponse>()
            .WithTags(EndpointTags.Contests);

        return apiGroup;
    }

    private static async Task<IResult> HandleAsync(IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeQuery()
        .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(TypedResults.Ok);

    private static ErrorOr<Query> InitializeQuery() => ErrorOrFactory.From(new Query());

    internal sealed record Query : IQuery<GetContestsResponse>;

    internal sealed class Handler(InMemoryContestRepository repository) : IQueryHandler<Query, GetContestsResponse>
    {
        public async Task<ErrorOr<GetContestsResponse>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            Contest[] contests = repository.Contests.OrderBy(contest => contest.ContestYear)
                .Select(contest => contest.ToContestDto())
                .ToArray();

            return ErrorOrFactory.From(new GetContestsResponse(contests));
        }
    }
}
