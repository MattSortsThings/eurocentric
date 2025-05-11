using ErrorOr;
using Eurocentric.Features.AdminApi.V0.Common;
using Eurocentric.Features.AdminApi.V0.Contests.Common;
using Eurocentric.Features.Shared.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V0.Contests;

public static class GetContests
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
            .Produces<Response>();

        return apiGroup;
    }

    public sealed record Response(Contest[] Contests);

    internal sealed record Query : IQuery<Response>;

    internal sealed class Handler : IQueryHandler<Query, Response>
    {
        public Task<ErrorOr<Response>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            Contest contest = new(Guid.NewGuid(), 2025, "Basel", ContestFormat.Liverpool);

            return Task.FromResult(ErrorOrFactory.From(new Response([contest])));
        }
    }

    private static class Endpoint
    {
        internal static async Task<Ok<Response>> HandleAsync(IRequestResponseBus bus,
            CancellationToken cancellationToken = default)
        {
            ErrorOr<Response> errorsOrResponse = await InitializeQuery()
                .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken));

            return TypedResults.Ok(errorsOrResponse.Value);
        }

        private static ErrorOr<Query> InitializeQuery() => ErrorOrFactory.From(new Query());
    }
}
