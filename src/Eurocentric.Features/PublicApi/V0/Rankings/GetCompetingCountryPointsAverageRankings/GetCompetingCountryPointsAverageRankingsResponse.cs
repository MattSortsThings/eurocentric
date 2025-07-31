using Eurocentric.Features.PublicApi.V0.Common.Dtos;

namespace Eurocentric.Features.PublicApi.V0.Rankings.GetCompetingCountryPointsAverageRankings;

public sealed record GetCompetingCountryPointsAverageRankingsResponse
{
    public required CompetingCountryPointsAverageRanking[] Rankings { get; init; }

    public required CompetingCountryPointsAverageFilters Filters { get; init; }

    public required PaginationMetadata Pagination { get; init; }
}
