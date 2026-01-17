using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Aggregates.Placeholders;

public sealed class Competitor
{
    public Guid CompetingCountryId { get; init; }

    public int PerformingSpot { get; init; }

    public BroadcastHalf BroadcastHalf { get; init; }

    public int FinishingSpot { get; init; }

    public List<PointsAward> PointsAwards { get; init; } = [];
}
