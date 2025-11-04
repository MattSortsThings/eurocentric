using Eurocentric.Domain.Enums;
using Eurocentric.Domain.V0.Queries.Rankings.Common;

namespace Eurocentric.Domain.Analytics.Rankings.VotingCountries;

public sealed record PointsAverageMetadata : PaginatedMetadata
{
    public string CompetingCountryCode { get; init; } = string.Empty;

    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public ContestStageFilter? ContestStage { get; init; }

    public VotingMethodFilter? VotingMethod { get; init; }
}
