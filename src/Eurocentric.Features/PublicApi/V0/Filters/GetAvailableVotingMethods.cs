using ErrorOr;
using Eurocentric.Features.PublicApi.V0.Common.Constants;
using Eurocentric.Features.PublicApi.V0.Common.Enums;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;

namespace Eurocentric.Features.PublicApi.V0.Filters;

public sealed record GetAvailableVotingMethodsResponse(VotingMethod[] VotingMethods);

internal static class GetAvailableVotingMethods
{
    internal static IEndpointRouteBuilder MapGetAvailableVotingMethods(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapGet("filters/voting-methods", HandleAsync)
            .WithName(EndpointIds.Filters.GetAvailableVotingMethods)
            .WithSummary("Get available voting methods")
            .WithDescription("Retrieves a list of all available 'votingMethod' query parameter values.")
            .HasApiVersion(0, 1)
            .HasApiVersion(0, 2)
            .Produces<GetAvailableVotingMethodsResponse>()
            .WithTags(EndpointTags.Filters);

        return apiGroup;
    }

    private static async Task<IResult> HandleAsync(IRequestResponseBus bus, CancellationToken cancellationToken = default) =>
        await InitializeQuery()
            .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
            .ToProblemOrResponseAsync(TypedResults.Ok);

    private static ErrorOr<Query> InitializeQuery() => ErrorOrFactory.From(new Query());

    internal sealed record Query : IQuery<GetAvailableVotingMethodsResponse>;

    internal sealed class Handler : IQueryHandler<Query, GetAvailableVotingMethodsResponse>
    {
        public async Task<ErrorOr<GetAvailableVotingMethodsResponse>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            return ErrorOrFactory.From(new GetAvailableVotingMethodsResponse(Enum.GetValues<VotingMethod>()));
        }
    }
}
