using Eurocentric.Apis.Public.V1.Enums;

namespace Eurocentric.Apis.Public.V1.Dtos.VotingCountryRankings;

public sealed record VotingCountryPointsAverageMetadata
{
    public string CompetingCountryCode { get; init; } = string.Empty;

    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public ContestStageFilter? ContestStage { get; init; }

    public VotingMethodFilter? VotingMethod { get; init; }

    public int PageIndex { get; init; }

    public int PageSize { get; init; }

    public bool Descending { get; init; }

    public int TotalItems { get; init; }

    public int TotalPages { get; init; }
}
