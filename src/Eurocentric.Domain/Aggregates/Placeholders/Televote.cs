namespace Eurocentric.Domain.Aggregates.Placeholders;

public sealed class Televote
{
    public Guid VotingCountryId { get; init; }

    public bool PointsAwarded { get; init; }
}
