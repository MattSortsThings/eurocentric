using Eurocentric.Features.PublicApi.V0.Common.Dtos;
using Eurocentric.Features.PublicApi.V0.Common.Enums;

namespace Eurocentric.Features.PublicApi.V0.CompetingCountryRankings.GetCompetingCountryPointsInRangeRankings;

public sealed record CompetingCountryPointsInRangeMetadata : PaginatedMetadata
{
    public required int MinPoints { get; init; }

    public required int MaxPoints { get; init; }

    public required int? MinYear { get; init; }

    public required int? MaxYear { get; init; }

    public required QueryableContestStage? ContestStage { get; init; }

    public required QueryableVotingMethod? VotingMethod { get; init; }

    public required string? VotingCountryCode { get; init; }
}
