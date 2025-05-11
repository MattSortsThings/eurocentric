using ErrorOr;
using Eurocentric.Features.PublicApi.V0.Common;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;

namespace Eurocentric.Features.PublicApi.V0.Filters;

public static class GetAvailableVotingMethods
{
    internal static IEndpointRouteBuilder MapGetAvailableVotingMethods(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapGet("filters/voting-methods", Endpoint.HandleAsync)
            .WithName("PublicApi.V0.GetAvailableVotingMethods")
            .HasApiVersion(0, 1)
            .HasApiVersion(0, 2)
            .WithSummary("Get available voting methods")
            .WithDescription("Retrieves all available voting methods.")
            .WithTags(EndpointTags.Filters)
            .Produces<Response>();

        return apiGroup;
    }

    public sealed record Response(VotingMethod[] AvailableVotingMethods);

    internal sealed record Query : IQuery<Response>;

    internal sealed class Handler : IQueryHandler<Query, Response>
    {
        public Task<ErrorOr<Response>> OnHandle(Query _, CancellationToken cancellationToken) =>
            Task.FromResult(ErrorOrFactory.From(new Response(Enum.GetValues<VotingMethod>())));
    }

    internal static class Endpoint
    {
        internal static async Task<Results<Ok<Response>, ProblemHttpResult>> HandleAsync(IRequestResponseBus bus,
            CancellationToken cancellationToken = default) => await InitializeQuery()
            .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
            .ToResultOrProblemAsync(TypedResults.Ok);

        private static ErrorOr<Query> InitializeQuery() => ErrorOrFactory.From(new Query());
    }
}
