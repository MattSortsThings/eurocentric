using System.Data;
using Dapper;
using Eurocentric.Components.DataAccess.Common;
using Eurocentric.Components.DataAccess.Dapper;
using Eurocentric.Domain.V0.Queries.Listings;

namespace Eurocentric.Components.Gateways.V0;

internal sealed class ListingsGateway(V0SprocRunner sprocRunner) : IListingsGateway
{
    public async Task<BroadcastResultListings> GetBroadcastResultListingsAsync(
        BroadcastResultQuery query,
        CancellationToken cancellationToken = default
    )
    {
        List<BroadcastResultListing> results = await sprocRunner
            .ExecuteAsync<BroadcastResultListing>(
                Sprocs.V0.GetBroadcastResultListings,
                MapToDynamicParameters(query),
                cancellationToken
            )
            .ConfigureAwait(false);

        BroadcastResultMetadata metadata = MapToMetadata(query);

        return new BroadcastResultListings(results, metadata);
    }

    private static DynamicParameters MapToDynamicParameters(BroadcastResultQuery query)
    {
        DynamicParameters dp = new();

        dp.Add("@contest_year", query.ContestYear, DbType.Int32, ParameterDirection.Input);

        dp.Add(
            "@contest_stage",
            query.ContestStage.ToString(),
            DbType.String,
            size: 10,
            direction: ParameterDirection.Input
        );

        return dp;
    }

    private static BroadcastResultMetadata MapToMetadata(BroadcastResultQuery query) =>
        new() { ContestYear = query.ContestYear, ContestStage = query.ContestStage };
}
