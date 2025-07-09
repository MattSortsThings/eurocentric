using ErrorOr;
using Eurocentric.Features.PublicApi.V0.Common.Contracts;
using Eurocentric.Features.Shared.Documentation;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using EndpointNames = Eurocentric.Features.PublicApi.V0.Common.Constants.EndpointNames;
using EndpointTags = Eurocentric.Features.PublicApi.V0.Common.Constants.EndpointTags;

namespace Eurocentric.Features.PublicApi.V0.Filters;

public sealed record GetContestStagesResponse(ContestStageFilter[] ContestStages) : IExampleProvider<GetContestStagesResponse>
{
    public static GetContestStagesResponse CreateExample() => new(Enum.GetValues<ContestStageFilter>());
}

internal static class GetContestStages
{
    internal static IEndpointRouteBuilder MapGetContestStages(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapGet("filters/contest-stages", ExecuteAsync)
            .WithName(EndpointNames.Filters.GetContestStages)
            .WithSummary("Get contest stages")
            .WithDescription("Retrieves a list of all the ContestStageFilter enum values.")
            .WithTags(EndpointTags.Filters)
            .HasApiVersion(0, 1)
            .HasApiVersion(0, 2)
            .Produces<GetContestStagesResponse>();

        return apiGroup;
    }

    private static async Task<Results<ProblemHttpResult, Ok<GetContestStagesResponse>>> ExecuteAsync(
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeQuery()
        .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(TypedResults.Ok);

    private static ErrorOr<Query> InitializeQuery() => ErrorOrFactory.From(new Query());

    internal sealed record Query : IQuery<GetContestStagesResponse>;

    internal sealed class Handler : IQueryHandler<Query, GetContestStagesResponse>
    {
        public Task<ErrorOr<GetContestStagesResponse>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            ContestStageFilter[] values = Enum.GetValues<ContestStageFilter>();

            return Task.FromResult(ErrorOrFactory.From(new GetContestStagesResponse(values)));
        }
    }
}
