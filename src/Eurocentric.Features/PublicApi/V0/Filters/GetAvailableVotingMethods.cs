using ErrorOr;
using Eurocentric.Features.PublicApi.V0.Common.Constants;
using Eurocentric.Features.PublicApi.V0.Common.Enums;
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
        apiGroup.MapGet("v0.1/filters/voting-methods", Endpoint.HandleAsync)
            .WithName("PublicApi.V0.1.GetAvailableVotingMethods")
            .WithSummary("Get available voting methods")
            .WithDescription("Retrieves a list of all available 'votingMethod' query parameter values.")
            .Produces<GetAvailableVotingMethodsResponse>()
            .WithTags(EndpointTags.Filters);

        apiGroup.MapGet("v0.2/filters/voting-methods", Endpoint.HandleAsync)
            .WithName("PublicApi.V0.2.GetAvailableVotingMethods")
            .WithSummary("Get available voting methods")
            .WithDescription("Retrieves a list of all available 'votingMethod' query parameter values.")
            .Produces<GetAvailableVotingMethodsResponse>()
            .WithTags(EndpointTags.Filters);

        return apiGroup;
    }

    internal sealed record Query : IQuery<GetAvailableVotingMethodsResponse>;

    internal sealed class Handler : IQueryHandler<Query, GetAvailableVotingMethodsResponse>
    {
        public async Task<ErrorOr<GetAvailableVotingMethodsResponse>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            return ErrorOrFactory.From(new GetAvailableVotingMethodsResponse(Enum.GetValues<VotingMethod>()));
        }
    }

    private static class Endpoint
    {
        public static async Task<IResult> HandleAsync(IRequestResponseBus bus, CancellationToken cancellationToken = default)
        {
            ErrorOr<GetAvailableVotingMethodsResponse> errorsOrResponse =
                await bus.Send(new Query(), cancellationToken: cancellationToken);

            return TypedResults.Ok(errorsOrResponse.Value);
        }
    }
}
