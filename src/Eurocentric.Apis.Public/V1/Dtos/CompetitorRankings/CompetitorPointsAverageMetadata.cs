using Eurocentric.Apis.Public.V1.Enums;

namespace Eurocentric.Apis.Public.V1.Dtos.CompetitorRankings;

public sealed record CompetitorPointsAverageMetadata
{
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
