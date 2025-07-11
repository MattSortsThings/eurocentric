using ErrorOr;
using Eurocentric.Features.PublicApi.V0.Common.Constants;
using Eurocentric.Features.PublicApi.V0.Common.Contracts;
using Eurocentric.Features.Shared.Documentation;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;

namespace Eurocentric.Features.PublicApi.V0.Filters;

public sealed record GetVotingMethodsResponse(VotingMethodFilter[] VotingMethods) : IExampleProvider<GetVotingMethodsResponse>
{
    public static GetVotingMethodsResponse CreateExample() => new(Enum.GetValues<VotingMethodFilter>());
}

internal static class GetVotingMethods
{
    internal static IEndpointRouteBuilder MapGetVotingMethods(this IEndpointRouteBuilder v0Group)
    {
        v0Group.MapGet("filters/voting-methods", ExecuteAsync)
            .WithName(EndpointNames.Filters.GetVotingMethods)
            .WithSummary("Get contest stages")
            .WithDescription("Retrieves a list of all the VotingMethodFilter enum values.")
            .WithTags(EndpointTags.Filters)
            .HasApiVersion(0, 1)
            .HasApiVersion(0, 2)
            .Produces<GetVotingMethodsResponse>();

        return v0Group;
    }

    private static async Task<Results<ProblemHttpResult, Ok<GetVotingMethodsResponse>>> ExecuteAsync(
        IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeQuery()
        .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(TypedResults.Ok);

    private static ErrorOr<Query> InitializeQuery() => ErrorOrFactory.From(new Query());

    internal sealed record Query : IQuery<GetVotingMethodsResponse>;

    internal sealed class Handler : IQueryHandler<Query, GetVotingMethodsResponse>
    {
        public Task<ErrorOr<GetVotingMethodsResponse>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            VotingMethodFilter[] values = Enum.GetValues<VotingMethodFilter>();

            return Task.FromResult(ErrorOrFactory.From(new GetVotingMethodsResponse(values)));
        }
    }
}
