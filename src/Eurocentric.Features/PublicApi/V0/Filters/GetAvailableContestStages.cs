using ErrorOr;
using Eurocentric.Features.PublicApi.V0.Common.Constants;
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
        apiGroup.MapGet("filters/contest-stages", HandleAsync)
            .WithName(EndpointIds.Filters.GetAvailableContestStages)
            .WithSummary("Get available contest stages")
            .WithDescription("Retrieves a list of all available 'contestStages' query parameter values.")
            .HasApiVersion(0, 1)
            .HasApiVersion(0, 2)
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
