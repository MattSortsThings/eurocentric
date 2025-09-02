using Eurocentric.Domain.Enums;
using Eurocentric.Domain.V0.Rankings.Common;

namespace Eurocentric.Domain.V0.Rankings.CompetingCountries;

public sealed record PointsInRangeQuery : PaginatedQuery
{
    public int MinPoints { get; init; }

    public int MaxPoints { get; init; }

    public int? MinYear { get; init; }

    public int? MaxYear { get; init; }

    public ContestStageFilter? ContestStage { get; init; }

    public VotingMethodFilter? VotingMethod { get; init; }

    public string? VotingCountryCode { get; init; }
}
