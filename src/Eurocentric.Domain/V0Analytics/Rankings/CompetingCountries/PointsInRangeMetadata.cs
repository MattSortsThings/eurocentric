using Eurocentric.Domain.Enums;
using Eurocentric.Domain.V0Analytics.Rankings.Common;

namespace Eurocentric.Domain.V0Analytics.Rankings.CompetingCountries;

public sealed record PointsInRangeMetadata : PaginatedMetadata
{
    public int MinPoints { get; init; }

    public int MaxPoints { get; init; }

    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public ContestStage[]? ContestStages { get; init; }

    public VotingMethod? VotingMethod { get; init; }

    public string? VotingCountryCode { get; init; }
}
