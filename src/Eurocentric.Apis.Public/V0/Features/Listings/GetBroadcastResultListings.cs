using CSharpFunctionalExtensions;
using Eurocentric.Apis.Public.V0.Config;
using Eurocentric.Apis.Public.V0.Dtos.Listings;
using Eurocentric.Apis.Public.V0.Enums;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Domain.Functional;
using Eurocentric.Domain.V0.Queries.Listings;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using IResult = Microsoft.AspNetCore.Http.IResult;
using ListingDto = Eurocentric.Apis.Public.V0.Dtos.Listings.BroadcastResultListing;
using ListingRecord = Eurocentric.Domain.V0.Queries.Listings.BroadcastResultListing;
using MetadataDto = Eurocentric.Apis.Public.V0.Dtos.Listings.BroadcastResultMetadata;
using MetadataRecord = Eurocentric.Domain.V0.Queries.Listings.BroadcastResultMetadata;

namespace Eurocentric.Apis.Public.V0.Features.Listings;

internal static class GetBroadcastResultListings
{
    private static Query ToQuery(this GetBroadcastResultListingsRequest request)
    {
        return new Query
        {
            ContestYear = request.ContestYear,
            ContestStage = request.ContestStage.ToDomainContestStage(),
        };
    }

    private static Ok<GetBroadcastResultListingsResponse> MapToOk(BroadcastResultListings listings)
    {
        (List<ListingRecord> listingRecords, MetadataRecord metadata) = listings;

        ListingDto[] listingDtos = listingRecords.Select(result => result.ToDto()).ToArray();
        MetadataDto metadataDto = metadata.ToDto();

        return TypedResults.Ok(new GetBroadcastResultListingsResponse(listingDtos, metadataDto));
    }

    private static async Task<IResult> ExecuteAsync(
        [AsParameters] GetBroadcastResultListingsRequest request,
        [FromServices] IRequestResponseBus bus,
        CancellationToken ct = default
    )
    {
        Result<BroadcastResultListings, IDomainError> result = await bus.Send(request.ToQuery(), cancellationToken: ct);

        return result.IsSuccess
            ? MapToOk(result.GetValueOrDefault())
            : throw new InvalidOperationException("Query failed.");
    }

    internal sealed class EndpointMapper : IEndpointMapper
    {
        public void MapEndpoint(RouteGroupBuilder routeBuilder)
        {
            routeBuilder
                .MapGet("listings/broadcast-result", ExecuteAsync)
                .WithName("PublicApi.V0.GetBroadcastResultListings")
                .AddedInVersion0Point2()
                .WithSummary("Get broadcast result listings")
                .WithDescription("Retrieves the results from the specified broadcast, ordered by finishing position.")
                .WithTags(EndpointConstants.Tags.Listings)
                .Produces<GetBroadcastResultListingsResponse>()
                .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }

    internal sealed record Query : BroadcastResultQuery, IQuery<BroadcastResultListings>;

    [UsedImplicitly]
    internal sealed class QueryHandler(IListingsGateway gateway) : IQueryHandler<Query, BroadcastResultListings>
    {
        public async Task<Result<BroadcastResultListings, IDomainError>> OnHandle(Query query, CancellationToken ct) =>
            await gateway.GetBroadcastResultListingsAsync(query, ct);
    }
}
