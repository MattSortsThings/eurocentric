using Eurocentric.Features.PublicApi.V0.Common.Dtos;
using Eurocentric.Features.PublicApi.V0.Common.Enums;

namespace Eurocentric.Features.PublicApi.V0.CompetingCountryRankings.GetCompetingCountryPointsInRangeRankings;

public sealed record CompetingCountryPointsInRangeMetadata : PaginatedMetadata
{
    public int MinPoints { get; init; }

    public int MaxPoints { get; init; }

    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public QueryableContestStage? ContestStage { get; init; }

    public QueryableVotingMethod? VotingMethod { get; init; }

    public string? VotingCountryCode { get; init; }
}
