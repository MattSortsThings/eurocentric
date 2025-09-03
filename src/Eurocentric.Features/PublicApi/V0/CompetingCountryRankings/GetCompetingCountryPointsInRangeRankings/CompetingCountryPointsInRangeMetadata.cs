using Eurocentric.Features.PublicApi.V0.Common.Dtos;
using Eurocentric.Features.PublicApi.V0.Common.Enums;
using Eurocentric.Features.Shared.Documentation;

namespace Eurocentric.Features.PublicApi.V0.CompetingCountryRankings.GetCompetingCountryPointsInRangeRankings;

public sealed record CompetingCountryPointsInRangeMetadata : PaginatedMetadata,
    IExampleProvider<CompetingCountryPointsInRangeMetadata>
{
    public int MinPoints { get; init; }

    public int MaxPoints { get; init; }

    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public QueryableContestStage? ContestStage { get; init; }

    public QueryableVotingMethod? VotingMethod { get; init; }

    public string? VotingCountryCode { get; init; }

    public static CompetingCountryPointsInRangeMetadata CreateExample() => new()
    {
        MinPoints = 1, MaxPoints = 12, PageIndex = 0, PageSize = 10, Descending = false, TotalItems = 50, TotalPages = 5
    };
}
