using Eurocentric.Features.PublicApi.V0.Common.Dtos;

namespace Eurocentric.Features.PublicApi.V0.Rankings.GetCompetingCountryPointsInRangeRankings;

public sealed record GetCompetingCountryPointsInRangeRankingsResponse
{
    public required CompetingCountryPointsInRangeRanking[] Rankings { get; init; }

    public required CompetingCountryPointsInRangeFilters Filters { get; init; }

    public required PaginationMetadata Pagination { get; init; }
}
