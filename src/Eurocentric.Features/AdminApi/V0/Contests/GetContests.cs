using ErrorOr;
using Eurocentric.Features.AdminApi.V0.Common;
using Eurocentric.Features.AdminApi.V0.Contests.Common;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V0.Contests;

public sealed record GetContestsResponse(Contest[] Contests);

internal static class GetContests
{
    internal static IEndpointRouteBuilder MapGetContests(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapGet("contests", Endpoint.HandleAsync)
            .WithName("AdminApi.V0.GetContests")
            .HasApiVersion(0, 1)
            .HasApiVersion(0, 2)
            .WithSummary("Get all contests")
            .WithDescription("Retrieves all existing contests, ordered by contest year.")
            .WithTags(EndpointTags.Contests)
            .Produces<GetContestsResponse>();

        return apiGroup;
    }

    internal sealed record Query : IQuery<GetContestsResponse>;

    internal sealed class Handler : IQueryHandler<Query, GetContestsResponse>
    {
        public Task<ErrorOr<GetContestsResponse>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            Contest contest = new(Guid.NewGuid(), 2025, "Basel", ContestFormat.Liverpool);

            return Task.FromResult(ErrorOrFactory.From(new GetContestsResponse([contest])));
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
