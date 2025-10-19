using Eurocentric.Apis.Public.V0.Dtos.Rankings.CompetingCountries;

namespace Eurocentric.Apis.Public.V0.Features.Rankings.CompetingCountries;

public sealed record GetCompetingCountryPointsAverageRankingsResponse(
    CompetingCountryPointsAverageRanking[] Rankings,
    CompetingCountryPointsAverageMetadata Metadata
);
