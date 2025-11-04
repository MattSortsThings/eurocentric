using Eurocentric.Apis.Public.V1.Dtos.Rankings.CompetingCountries;

namespace Eurocentric.Apis.Public.V1.Features.Rankings.CompetingCountries;

public sealed record GetCompetingCountryPointsAverageRankingsResponse(
    CompetingCountryPointsAverageRanking[] Rankings,
    CompetingCountryPointsAverageMetadata Metadata
);
