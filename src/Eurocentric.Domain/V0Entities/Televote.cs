namespace Eurocentric.Domain.V0Entities;

public sealed record Televote
{
    public Guid VotingCountryId { get; init; }

    public bool PointsAwarded { get; init; }
}
