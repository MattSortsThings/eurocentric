using Eurocentric.Domain.Enums;
using Eurocentric.Domain.V0.Queries.Rankings.Common;

namespace Eurocentric.Domain.Analytics.Rankings.CompetingCountries;

public sealed record PointsAverageMetadata : PaginatedMetadata
{
    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public ContestStageFilter? ContestStage { get; init; }

    public string? VotingCountryCode { get; init; }

    public VotingMethodFilter? VotingMethod { get; init; }
}
