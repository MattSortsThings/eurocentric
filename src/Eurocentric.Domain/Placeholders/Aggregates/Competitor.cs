using Eurocentric.Domain.Enums;

namespace Eurocentric.Domain.Placeholders.Aggregates;

public sealed class Competitor
{
    public required Guid CompetingCountryId { get; init; }

    public required int PerformingSpot { get; init; }

    public required BroadcastHalf BroadcastHalf { get; init; }

    public required int FinishingSpot { get; init; }

    public required List<PointsAward> PointsAwards { get; init; }
}
