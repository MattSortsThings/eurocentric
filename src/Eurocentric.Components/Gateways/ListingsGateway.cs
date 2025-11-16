using CSharpFunctionalExtensions;
using Eurocentric.Components.DataAccess.Common;
using Eurocentric.Components.DataAccess.Dapper;
using Eurocentric.Domain.Analytics.Listings;
using Eurocentric.Domain.Core;

namespace Eurocentric.Components.Gateways;

internal sealed class ListingsGateway(ListSprocRunner sprocRunner) : IListingsGateway
{
    public async Task<Result<BroadcastResultListings, IDomainError>> GetBroadcastResultListingsAsync(
        BroadcastResultQuery query,
        CancellationToken cancellationToken = default
    ) => await RunSprocAsync(query, cancellationToken);

    public async Task<Result<CompetingCountryResultListings, IDomainError>> GetCompetingCountryResultListingsAsync(
        CompetingCountryResultQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return await Result
            .Success<CompetingCountryResultQuery, IDomainError>(query)
            .Ensure(ListingsInvariants.LegalCompetingCountryFiltering)
            .Bind(queryParams => RunSprocAsync(queryParams, cancellationToken));
    }

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

    public async Task<Result<VotingCountryPointsListings, IDomainError>> GetVotingCountryPointsListingsAsync(
        VotingCountryPointsQuery query,
        CancellationToken cancellationToken = default
    )
    {
        return await Result
            .Success<VotingCountryPointsQuery, IDomainError>(query)
            .Ensure(ListingsInvariants.LegalVotingCountryFiltering)
            .Bind(queryParams => RunSprocAsync(queryParams, cancellationToken));
    }

    private async Task<Result<BroadcastResultListings, IDomainError>> RunSprocAsync(
        BroadcastResultQuery query,
        CancellationToken cancellationToken = default
    )
    {
        ListingsDynamicParameters dynamicParameters = ListingsDynamicParameters.From(query);

        List<BroadcastResultListing> resultListings = await sprocRunner.ExecuteAsync<BroadcastResultListing>(
            Sprocs.Dbo.GetBroadcastResultListings,
            dynamicParameters,
            cancellationToken
        );

        BroadcastResultMetadata metadata = MapToMetadata(query);

        return new BroadcastResultListings(resultListings, metadata);
    }

    private async Task<Result<CompetingCountryResultListings, IDomainError>> RunSprocAsync(
        CompetingCountryResultQuery query,
        CancellationToken cancellationToken = default
    )
    {
        ListingsDynamicParameters dynamicParameters = ListingsDynamicParameters.From(query);

        List<CompetingCountryResultListing> resultListings =
            await sprocRunner.ExecuteAsync<CompetingCountryResultListing>(
                Sprocs.Dbo.GetCompetingCountryResultListings,
                dynamicParameters,
                cancellationToken
            );

        CompetingCountryResultMetadata metadata = MapToMetadata(query);

        return new CompetingCountryResultListings(resultListings, metadata);
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

    private async Task<Result<VotingCountryPointsListings, IDomainError>> RunSprocAsync(
        VotingCountryPointsQuery query,
        CancellationToken cancellationToken = default
    )
    {
        ListingsDynamicParameters dynamicParameters = ListingsDynamicParameters.From(query);

        List<VotingCountryPointsListing> juryListings = await sprocRunner.ExecuteAsync<VotingCountryPointsListing>(
            Sprocs.Dbo.GetVotingCountryJuryPointsListings,
            dynamicParameters,
            cancellationToken
        );

        List<VotingCountryPointsListing> televoteListings = await sprocRunner.ExecuteAsync<VotingCountryPointsListing>(
            Sprocs.Dbo.GetVotingCountryTelevotePointsListings,
            dynamicParameters,
            cancellationToken
        );

        VotingCountryPointsMetadata metadata = MapToMetadata(query);

        return new VotingCountryPointsListings(juryListings, televoteListings, metadata);
    }

    private static BroadcastResultMetadata MapToMetadata(BroadcastResultQuery query) =>
        new() { ContestYear = query.ContestYear, ContestStage = query.ContestStage };

    private static CompetingCountryResultMetadata MapToMetadata(CompetingCountryResultQuery query) =>
        new() { CompetingCountryCode = query.CompetingCountryCode };

    private static CompetingCountryPointsMetadata MapToMetadata(CompetingCountryPointsQuery query)
    {
        return new CompetingCountryPointsMetadata
        {
            ContestYear = query.ContestYear,
            ContestStage = query.ContestStage,
            CompetingCountryCode = query.CompetingCountryCode,
        };
    }

    private static VotingCountryPointsMetadata MapToMetadata(VotingCountryPointsQuery query)
    {
        return new VotingCountryPointsMetadata
        {
            ContestYear = query.ContestYear,
            ContestStage = query.ContestStage,
            VotingCountryCode = query.VotingCountryCode,
        };
    }
}
