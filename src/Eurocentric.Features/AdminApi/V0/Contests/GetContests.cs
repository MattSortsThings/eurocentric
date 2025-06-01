using ErrorOr;
using Eurocentric.Features.AdminApi.V0.Common.Constants;
using Eurocentric.Features.AdminApi.V0.Common.Dtos;
using Eurocentric.Features.AdminApi.V0.Common.Mapping;
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
        apiGroup.MapGet("v0.1/contests", Endpoint.HandleAsync)
            .WithName("AdminApi.V0.1.GetContests")
            .WithSummary("Get all contests")
            .WithDescription("Retrieves a list of all existing contests, ordered by contest year.")
            .Produces<GetContestsResponse>()
            .WithTags(EndpointTags.Contests);

        apiGroup.MapGet("v0.2/contests", Endpoint.HandleAsync)
            .WithName("AdminApi.V0.2.GetContests")
            .WithSummary("Get all contests")
            .WithDescription("Retrieves a list of all existing contests, ordered by contest year.")
            .Produces<GetContestsResponse>()
            .WithTags(EndpointTags.Contests);

        return apiGroup;
    }

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

    private static class Endpoint
    {
        internal static async Task<IResult> HandleAsync(IRequestResponseBus bus,
            CancellationToken cancellationToken = default)
        {
            ErrorOr<GetContestsResponse> errorsOrResponse = await bus.Send(new Query(), cancellationToken: cancellationToken);

            return TypedResults.Ok(errorsOrResponse.Value);
        }
    }
}
