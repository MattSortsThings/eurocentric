using ErrorOr;
using Eurocentric.Features.AdminApi.V0.Common;
using Eurocentric.Features.AdminApi.V0.Contests.Common;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V0.Contests;

public sealed record GetContestResponse(Contest Contest);

internal static class GetContest
{
    internal static IEndpointRouteBuilder MapGetContest(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapGet("contests/{contestId:guid}", Endpoint.HandleAsync)
            .WithName("AdminApi.V0.GetContest")
            .HasApiVersion(0, 1)
            .HasApiVersion(0, 2)
            .WithSummary("Get a contest")
            .WithDescription("Retrieves a single contest.")
            .WithTags(EndpointTags.Contests)
            .Produces<GetContestResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        return apiGroup;
    }

    internal sealed record Query(Guid ContestId) : IQuery<GetContestResponse>;

    internal sealed class Handler : IQueryHandler<Query, GetContestResponse>
    {
        public Task<ErrorOr<GetContestResponse>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            Contest contest = new(query.ContestId, 2025, "Basel", ContestFormat.Liverpool);

            return Task.FromResult(ErrorOrFactory.From(new GetContestResponse(contest)));
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
