using ErrorOr;
using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Common.DomainMapping;
using Eurocentric.Features.AdminApi.V1.Common.Dtos;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Infrastructure.EFCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using SlimMessageBus;

namespace Eurocentric.Features.AdminApi.V1.Broadcasts;

public sealed record GetBroadcastsResponse(Broadcast[] Broadcasts);

internal static class GetBroadcasts
{
    internal static IEndpointRouteBuilder MapGetBroadcasts(this IEndpointRouteBuilder apiGroup)
    {
        apiGroup.MapGet("broadcasts", HandleAsync)
            .WithName(EndpointIds.Broadcasts.GetBroadcasts)
            .WithSummary("Get all broadcasts")
            .WithDescription("Retrieves a list of all existing broadcasts, ordered by broadcast date.")
            .HasApiVersion(1, 0)
            .Produces<GetBroadcastsResponse>()
            .WithTags(EndpointTags.Broadcasts);

        return apiGroup;
    }

    private static async Task<Results<ProblemHttpResult, Ok<GetBroadcastsResponse>>> HandleAsync(IRequestResponseBus bus,
        CancellationToken cancellationToken = default) => await InitializeQuery()
        .ThenAsync(query => bus.Send(query, cancellationToken: cancellationToken))
        .ToProblemOrResponseAsync(TypedResults.Ok);

    private static ErrorOr<Query> InitializeQuery() => ErrorOrFactory.From(new Query());

    internal sealed record Query : IQuery<GetBroadcastsResponse>;

    internal sealed class Handler(AppDbContext dbContext) : IQueryHandler<Query, GetBroadcastsResponse>
    {
        public async Task<ErrorOr<GetBroadcastsResponse>> OnHandle(Query _, CancellationToken cancellationToken)
        {
            Broadcast[] broadcasts = await dbContext.Broadcasts
                .AsNoTracking()
                .AsSplitQuery()
                .OrderBy(broadcast => broadcast.BroadcastDate)
                .Select(broadcast => broadcast.ToBroadcastDto())
                .ToArrayAsync(cancellationToken);

            return ErrorOrFactory.From(new GetBroadcastsResponse(broadcasts));
        }
    }
}
