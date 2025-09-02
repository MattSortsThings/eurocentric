namespace Eurocentric.Features.PublicApi.V0.CompetingCountryRankings.GetCompetingCountryPointsInRangeRankings;

public sealed record GetCompetingCountryPointsInRangeRankingsResponse(
    CompetingCountryPointsInRangeRanking[] Rankings,
    CompetingCountryPointsInRangeMetadata Metadata);
