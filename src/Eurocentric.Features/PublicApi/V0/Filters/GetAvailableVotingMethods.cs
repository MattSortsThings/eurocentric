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

public sealed record GetAvailableVotingMethodsResponse(VotingMethod[] AvailableVotingMethods);

internal static class GetAvailableVotingMethods
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
            .Produces<GetAvailableVotingMethodsResponse>();

        return apiGroup;
    }

    internal sealed record Query : IQuery<GetAvailableVotingMethodsResponse>;

    internal sealed class Handler : IQueryHandler<Query, GetAvailableVotingMethodsResponse>
    {
        public Task<ErrorOr<GetAvailableVotingMethodsResponse>> OnHandle(Query _, CancellationToken cancellationToken) =>
            Task.FromResult(ErrorOrFactory.From(new GetAvailableVotingMethodsResponse(Enum.GetValues<VotingMethod>())));
    }

    private static class Endpoint
    {
        internal static async Task<Results<Ok<GetAvailableVotingMethodsResponse>, ProblemHttpResult>> HandleAsync(
            IRequestResponseBus bus,
            CancellationToken cancellationToken = default) => await InitializeQuery()
            .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
            .ToResultOrProblemAsync(TypedResults.Ok);

        private static ErrorOr<Query> InitializeQuery() => ErrorOrFactory.From(new Query());
    }
}
