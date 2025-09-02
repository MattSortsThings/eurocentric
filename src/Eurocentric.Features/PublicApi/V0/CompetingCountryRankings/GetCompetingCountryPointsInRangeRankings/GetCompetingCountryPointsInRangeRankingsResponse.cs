namespace Eurocentric.Features.PublicApi.V0.CompetingCountryRankings.GetCompetingCountryPointsInRangeRankings;

public record GetCompetingCountryPointsInRangeRankingsResponse(
    CompetingCountryPointsInRangeRanking[] Rankings,
    CompetingCountryPointsInRangeMetadata Metadata);
