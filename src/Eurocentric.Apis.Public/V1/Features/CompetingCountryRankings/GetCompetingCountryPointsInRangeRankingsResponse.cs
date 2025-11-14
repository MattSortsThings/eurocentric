using Eurocentric.Apis.Public.V1.Dtos.CompetingCountryRankings;

namespace Eurocentric.Apis.Public.V1.Features.CompetingCountryRankings;

public sealed record GetCompetingCountryPointsInRangeRankingsResponse(
    CompetingCountryPointsInRangeRanking[] Rankings,
    CompetingCountryPointsInRangeMetadata Metadata
);
