using ErrorOr;
using Eurocentric.Features.PublicApi.V0.Common.Enums;
using Eurocentric.Features.Shared.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using EndpointTags = Eurocentric.Features.PublicApi.V0.Common.Constants.EndpointTags;

namespace Eurocentric.Features.PublicApi.V0.Filters;

public sealed record GetAvailableContestStagesResponse(ContestStages[] ContestStages);

internal static class GetAvailableContestStages
{
    internal static IEndpointRouteBuilder MapGetAvailableContestStages(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapGet("v0.1/filters/contest-stages", HandleAsync)
            .WithName("PublicApi.V0.1.GetAvailableContestStages")
            .WithSummary("Get available contest stages")
            .WithDescription("Retrieves a list of all available 'contestStages' query parameter values.")
            .Produces<GetAvailableContestStagesResponse>()
            .WithTags(EndpointTags.Filters);

        apiGroup.MapGet("v0.2/filters/contest-stages", HandleAsync)
            .WithName("PublicApi.V0.2.GetAvailableContestStages")
            .WithSummary("Get available contest stages")
            .WithDescription("Retrieves a list of all available 'contestStages' query parameter values.")
            .Produces<GetAvailableContestStagesResponse>()
            .WithTags(EndpointTags.Filters);

        return apiGroup;
    }

    private static async Task<IResult> HandleAsync(IRequestResponseBus bus, CancellationToken cancellationToken = default)
    {
        ErrorOr<GetAvailableContestStagesResponse> errorsOrResponse =
            await bus.Send(new Query(), cancellationToken: cancellationToken);

        return TypedResults.Ok(errorsOrResponse.Value);
    }

    internal sealed record Query : IQuery<GetAvailableContestStagesResponse>;

    internal sealed class Handler : IQueryHandler<Query, GetAvailableContestStagesResponse>
    {
        public async Task<ErrorOr<GetAvailableContestStagesResponse>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            return ErrorOrFactory.From(new GetAvailableContestStagesResponse(Enum.GetValues<ContestStages>()));
        }
    }
}
