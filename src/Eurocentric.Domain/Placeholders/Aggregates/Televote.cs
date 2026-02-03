namespace Eurocentric.Domain.Placeholders.Aggregates;

public sealed class Televote
{
    public required Guid VotingCountryId { get; init; }

    public required bool PointsAwarded { get; init; }
}
