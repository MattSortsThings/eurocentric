using Eurocentric.Features.PublicApi.V0.Common.Enums;

namespace Eurocentric.Features.PublicApi.V0.Rankings.GetCompetingCountryPointsInRangeRankings;

public sealed record CompetingCountryPointsInRangeFilters
{
    public int MinPoints { get; init; }

    public int MaxPoints { get; init; }

    public QueryableVotingMethod VotingMethod { get; init; }

    public QueryableContestStage ContestStage { get; init; }

    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public string? VotingCountryCode { get; init; }
}
