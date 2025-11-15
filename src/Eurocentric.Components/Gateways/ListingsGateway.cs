using CSharpFunctionalExtensions;
using Eurocentric.Components.DataAccess.Common;
using Eurocentric.Components.DataAccess.Dapper;
using Eurocentric.Domain.Analytics.Listings;
using Eurocentric.Domain.Core;

namespace Eurocentric.Components.Gateways;

internal sealed class ListingsGateway(ListSprocRunner sprocRunner) : IListingsGateway
{
    public async Task<Result<CompetingCountryPointsListings, IDomainError>> GetCompetingCountryPointsListingsAsync(
        CompetingCountryPointsQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return await Result
            .Success<CompetingCountryPointsQuery, IDomainError>(query)
            .Ensure(ListingsInvariants.LegalCompetingCountryFiltering)
            .Bind(queryParams => RunSprocAsync(queryParams, cancellationToken));
    }

    private async Task<Result<CompetingCountryPointsListings, IDomainError>> RunSprocAsync(
        CompetingCountryPointsQuery query,
        CancellationToken cancellationToken = default
    )
    {
        ListingsDynamicParameters dynamicParameters = ListingsDynamicParameters.From(query);

        List<CompetingCountryPointsListing> juryListings =
            await sprocRunner.ExecuteAsync<CompetingCountryPointsListing>(
                Sprocs.Dbo.GetCompetingCountryJuryPointsListings,
                dynamicParameters,
                cancellationToken
            );

        List<CompetingCountryPointsListing> televoteListings =
            await sprocRunner.ExecuteAsync<CompetingCountryPointsListing>(
                Sprocs.Dbo.GetCompetingCountryTelevotePointsListings,
                dynamicParameters,
                cancellationToken
            );

        CompetingCountryPointsMetadata metadata = MapToMetadata(query);

        return new CompetingCountryPointsListings(juryListings, televoteListings, metadata);
    }

    private static CompetingCountryPointsMetadata MapToMetadata(CompetingCountryPointsQuery query)
    {
        return new CompetingCountryPointsMetadata
        {
            ContestYear = query.ContestYear,
            ContestStage = query.ContestStage,
            CompetingCountryCode = query.CompetingCountryCode,
        };
    }
}
