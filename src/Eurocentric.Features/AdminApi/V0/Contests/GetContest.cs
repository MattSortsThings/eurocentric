using ErrorOr;
using Eurocentric.Features.AdminApi.V0.Common.Constants;
using Eurocentric.Features.AdminApi.V0.Common.Dtos;
using Eurocentric.Features.AdminApi.V0.Common.Mapping;
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
        apiGroup.MapGet("v0.1/contests/{contestId:guid}", Endpoint.HandleAsync)
            .WithName("AdminApi.V0.1.GetContest")
            .WithSummary("Get a contest")
            .WithDescription("Retrieves a single contest.")
            .Produces<GetContestResponse>()
            .WithTags(EndpointTags.Contests);

        apiGroup.MapGet("v0.2/contests/{contestId:guid}", Endpoint.HandleAsync)
            .WithName("AdminApi.V0.2.GetContest")
            .WithSummary("Get a contest")
            .WithDescription("Retrieves a single contest.")
            .Produces<GetContestResponse>()
            .WithTags(EndpointTags.Contests);

        return apiGroup;
    }

    internal sealed record Query(Guid ContestId) : IQuery<GetContestResponse>;

    internal sealed class Handler(InMemoryContestRepository repository) : IQueryHandler<Query, GetContestResponse>
    {
        public async Task<ErrorOr<GetContestResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            Contest contest = repository.Contests
                .Where(contest => contest.Id == query.ContestId)
                .Select(contest => contest.ToContestDto())
                .First();

            return ErrorOrFactory.From(new GetContestResponse(contest));
        }
    }

    private static class Endpoint
    {
        internal static async Task<IResult> HandleAsync([FromRoute(Name = "contestId")] Guid contestId,
            IRequestResponseBus bus,
            CancellationToken cancellationToken = default)
        {
            ErrorOr<GetContestResponse> errorsOrResponse = await bus.Send(new Query(contestId),
                cancellationToken: cancellationToken);

            return TypedResults.Ok(errorsOrResponse.Value);
        }
    }
}
