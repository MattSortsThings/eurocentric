using ErrorOr;
using Eurocentric.Features.AdminApi.V0.Common;
using Eurocentric.Features.AdminApi.V0.Contests.Common;
using Eurocentric.Features.Shared.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V0.Contests;

public static class GetContest
{
    internal static IEndpointRouteBuilder MapGetContest(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapGet("v0.1/contests/{contestId:guid}", Endpoint.HandleAsync)
            .WithName("AdminApi.V0.1.GetContest")
            .WithSummary("Get a contest")
            .WithDescription("Retrieves a single contest.")
            .WithTags(EndpointTags.Contests)
            .Produces<Response>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        apiGroup.MapGet("v0.2/contests/{contestId:guid}", Endpoint.HandleAsync)
            .WithName("AdminApi.V0.2.GetContest")
            .WithSummary("Get a contest")
            .WithDescription("Retrieves a single contest.")
            .WithTags(EndpointTags.Contests)
            .Produces<Response>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        return apiGroup;
    }

    public sealed record Response(Contest Contest);

    internal sealed record Query(Guid ContestId) : IQuery<Response>;

    internal sealed class Handler : IQueryHandler<Query, Response>
    {
        public Task<ErrorOr<Response>> OnHandle(Query query, CancellationToken cancellationToken)
        {
            Contest contest = new(query.ContestId, 2025, "Basel", ContestFormat.Liverpool);

            return Task.FromResult(ErrorOrFactory.From(new Response(contest)));
        }
    }

    private static class Endpoint
    {
        internal static async Task<Ok<Response>> HandleAsync([FromRoute(Name = "contestId")] Guid contestId,
            IRequestResponseBus bus,
            CancellationToken cancellationToken = default)
        {
            ErrorOr<Response> errorsOrResponse = await InitializeQuery(contestId)
                .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken));

            return TypedResults.Ok(errorsOrResponse.Value);
        }

        private static ErrorOr<Query> InitializeQuery(Guid contestId) => ErrorOrFactory.From(new Query(contestId));
    }
}
